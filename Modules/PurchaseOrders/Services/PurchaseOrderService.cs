using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Infrastructure.Persistence;
using warehouse_management_system.Modules.Approvals.Interfaces;
using warehouse_management_system.Modules.Approvals.DTOs;
using warehouse_management_system.Modules.PurchaseOrders.DTOs;
using warehouse_management_system.Modules.PurchaseOrders.Entities;
using warehouse_management_system.Modules.PurchaseOrders.Interfaces;

namespace warehouse_management_system.Modules.PurchaseOrders.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _uow;
    private readonly IApprovalService _approvalService;

    public PurchaseOrderService(
        ApplicationDbContext context,
        IUnitOfWork uow,
        IApprovalService approvalService)
    {
        _context = context;
        _uow = uow;
        _approvalService = approvalService;
    }

    public async Task<Guid> CreateAsync(CreatePurchaseOrderDto dto)
    {
        var po = new PurchaseOrder
        {
            VendorId = dto.VendorId,
            WarehouseId = dto.WarehouseId,
            PONumber = $"PO-{DateTime.UtcNow.Ticks}",
            Status = "PendingApproval"
        };

        await _context.PurchaseOrders.AddAsync(po);
        await _context.SaveChangesAsync();

        // 🔥 Create Approval Request Automatically
        await _approvalService.CreateAsync(
            new CreateApprovalRequestDto
            {
                ModuleName = "PurchaseOrder",
                ReferenceId = po.Id
            });

        return po.Id;
    }

    public async Task<bool> AddLineAsync(Guid poId, AddPOLineDto dto)
    {
        var line = new PurchaseOrderLine
        {
            PurchaseOrderId = poId,
            ItemId = dto.ItemId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice
        };

        await _context.PurchaseOrderLines.AddAsync(line);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllAsync()
        => _context.PurchaseOrders.ToList();

    public async Task<bool> UpdateStatusAsync(Guid poId, string status)
    {
        var po = await _context.PurchaseOrders.FindAsync(poId);
        if (po == null) return false;

        // 🚫 Cannot manually approve
        if (status == "Approved")
            return false;

        po.Status = status;
        await _context.SaveChangesAsync();

        return true;
    }
}

