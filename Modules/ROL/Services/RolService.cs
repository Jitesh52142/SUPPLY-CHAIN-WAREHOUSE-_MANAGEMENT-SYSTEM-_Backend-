using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.ROL.Entities;
using warehouse_management_system.Modules.ROL.Interfaces;
using warehouse_management_system.Modules.Notifications.Interfaces;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.PurchaseOrders.Entities;
using warehouse_management_system.Modules.Approvals.Interfaces;
using warehouse_management_system.Modules.Approvals.DTOs;

namespace warehouse_management_system.Modules.ROL.Services;

public class RolService : IRolService
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IApprovalService _approvalService;

    public RolService(
        ApplicationDbContext context,
        INotificationService notificationService,
        IApprovalService approvalService)
    {
        _context = context;
        _notificationService = notificationService;
        _approvalService = approvalService;
    }

    public async Task CheckStockAsync()
    {
        var items = await _context.Items.ToListAsync();

        foreach (var item in items)
        {
            var currentStock = await _context.Batches
                .Where(b => b.ItemId == item.Id)
                .SumAsync(b => (decimal?)b.AvailableQuantity) ?? 0;

            var last90Days = DateTime.UtcNow.AddDays(-90);

            var dailyUsage = await _context.StockLedgers
                .Where(x => x.ItemId == item.Id &&
                            x.MovementType == "OUT" &&
                            x.CreatedAt >= last90Days)
                .GroupBy(x => x.CreatedAt.Date)
                .Select(g => g.Sum(x => x.Quantity))
                .ToListAsync();

            if (!dailyUsage.Any())
                continue;

            var avgDailyUsage = dailyUsage.Average();
            var stdDev = CalculateStandardDeviation(dailyUsage);
            var safetyStock = stdDev * 1.65m;

            var leadTime = item.LeadTimeDays > 0 ? item.LeadTimeDays : 7;

            var calculatedRol =
                (avgDailyUsage * leadTime) + safetyStock;

            if (currentStock > calculatedRol)
                continue;

            var existingRequest =
                await _context.ReorderRequests
                    .FirstOrDefaultAsync(r =>
                        r.ItemId == item.Id &&
                        r.Status == "Pending");

            if (existingRequest != null)
                continue;

            using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var reorder = new ReorderRequest
                {
                    ItemId = item.Id,
                    CurrentStock = currentStock,
                    MinimumLevel = calculatedRol,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                await _context.ReorderRequests.AddAsync(reorder);

                var optimalQty =
                    item.MaximumStockLevel - currentStock;

                if (optimalQty <= 0)
                    optimalQty = avgDailyUsage * leadTime;

                // Vendor selection
                var vendorId = await _context.Vendors
                    .Where(v => v.IsActive)
                    .Select(v => v.Id)
                    .FirstOrDefaultAsync();

                if (vendorId == Guid.Empty)
                    throw new Exception("No active vendor found.");

                // Warehouse selection (first active)
                var warehouseId = await _context.Warehouses
                    .Where(w => w.IsActive)
                    .Select(w => w.Id)
                    .FirstOrDefaultAsync();

                if (warehouseId == Guid.Empty)
                    throw new Exception("No active warehouse found.");

                var po = new PurchaseOrder
                {
                    VendorId = vendorId,
                    WarehouseId = warehouseId,
                    PONumber = $"PO-{DateTime.UtcNow.Ticks}",
                    OrderDate = DateTime.UtcNow,
                    Status = "PendingApproval"
                };

                po.Lines.Add(new PurchaseOrderLine
                {
                    ItemId = item.Id,
                    Quantity = optimalQty,
                    UnitPrice = 0
                });

                await _context.PurchaseOrders.AddAsync(po);
                await _context.SaveChangesAsync();

                reorder.PurchaseOrderId = po.Id;

                await _approvalService.CreateAsync(
                    new CreateApprovalRequestDto
                    {
                        ModuleName = "PurchaseOrder",
                        ReferenceId = po.Id
                    });

                await _notificationService.CreateAsync(
                    new CreateNotificationDto
                    {
                        Title = "ROL Triggered",
                        Message =
                            $"Item {item.Name} triggered ROL. PO Created.",
                        TargetRole = "Admin"
                    });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private decimal CalculateStandardDeviation(List<decimal> values)
    {
        var avg = values.Average();
        var variance = values.Sum(v => (v - avg) * (v - avg)) / values.Count;
        return (decimal)Math.Sqrt((double)variance);
    }
}