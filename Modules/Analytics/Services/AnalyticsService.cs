using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Analytics.Interfaces;

namespace warehouse_management_system.Modules.Analytics.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1️⃣ Dashboard Summary
    public async Task<object> GetDashboardSummaryAsync()
    {
        return new
        {
            TotalItems = await _context.Items.CountAsync(),
            TotalVendors = await _context.Vendors.CountAsync(),
            TotalWarehouses = await _context.Warehouses.CountAsync(),
            PendingApprovals = await _context.ApprovalRequests
                .CountAsync(x => x.Status == "Pending"),
            LowStockItems = await _context.ReorderRequests
                .CountAsync(x => x.Status == "Pending")
        };
    }

    // 2️⃣ Stock Summary (Batch-Based)
    public async Task<IEnumerable<object>> GetStockSummaryAsync()
    {
        return await _context.Batches
            .GroupBy(x => new { x.ItemId, x.WarehouseId })
            .Select(g => new
            {
                g.Key.ItemId,
                g.Key.WarehouseId,
                CurrentStock = g.Sum(x => x.AvailableQuantity)
            })
            .ToListAsync();
    }

    // 3️⃣ Movement History
    public async Task<IEnumerable<object>> GetMovementHistoryAsync(Guid itemId)
    {
        return await _context.StockLedgers
            .Where(x => x.ItemId == itemId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new
            {
                x.ItemId,
                x.WarehouseId,
                x.MovementType,
                x.Quantity,
                x.CreatedAt
            })
            .ToListAsync();
    }

    // 4️⃣ Vendor Purchase Analytics
    public async Task<IEnumerable<object>> GetVendorPurchaseAnalyticsAsync()
    {
        return await _context.PurchaseOrders
            .GroupBy(x => x.VendorId)
            .Select(g => new
            {
                VendorId = g.Key,
                TotalOrders = g.Count(),
                ApprovedOrders = g.Count(x => x.Status == "Approved")
            })
            .ToListAsync();
    }

    // 5️⃣ Low Stock Report
    public async Task<IEnumerable<object>> GetLowStockReportAsync()
    {
        return await _context.ReorderRequests
            .Where(x => x.Status == "Pending")
            .Select(x => new
            {
                x.ItemId,
                x.CurrentStock,
                x.MinimumLevel,
                x.CreatedAt
            })
            .ToListAsync();
    }
}