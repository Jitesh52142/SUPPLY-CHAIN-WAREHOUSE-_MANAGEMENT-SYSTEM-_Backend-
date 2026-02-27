using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.GRN.DTOs;
using warehouse_management_system.Modules.GRN.Entities;
using warehouse_management_system.Modules.GRN.Interfaces;
using warehouse_management_system.Modules.Inventory.Entities;
using warehouse_management_system.Modules.InventoryMovement.Entities;

namespace warehouse_management_system.Modules.GRN.Services;

public class GRNService : IGRNService
{
    private readonly ApplicationDbContext _context;

    public GRNService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateGRNAsync(CreateGRNDto dto)
    {
        var grn = new GRNEntity
        {
            PurchaseOrderId = dto.PurchaseOrderId,
            GRNNumber = $"GRN-{DateTime.UtcNow.Ticks}"
        };

        await _context.GRNs.AddAsync(grn);
        await _context.SaveChangesAsync();

        return grn.Id;
    }

    public async Task<bool> AddItemAsync(Guid grnId, GRNItemDto dto)
    {
        if (dto.QuantityReceived <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var grn = await _context.GRNs.FindAsync(grnId);
            if (grn == null)
                throw new Exception("GRN not found.");

            // 🔹 Create GRN Line
            var line = new GRNLine
            {
                GRNId = grnId,
                ItemId = dto.ItemId,
                QuantityReceived = dto.QuantityReceived
            };

            await _context.GRNLines.AddAsync(line);

            // 🔹 Create Batch
            var batch = new Batch
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.WarehouseId,
                BatchNumber = dto.BatchNumber,
                ExpiryDate = dto.ExpiryDate,
                QuantityReceived = dto.QuantityReceived,
                AvailableQuantity = dto.QuantityReceived
            };

            await _context.Batches.AddAsync(batch);

            // 🔹 Ledger Entry (IN movement)
            await _context.StockLedgers.AddAsync(new StockLedger
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.WarehouseId,
                Quantity = dto.QuantityReceived,
                MovementType = "IN",
                ReferenceNumber = $"GRN-{DateTime.UtcNow.Ticks}"
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<GRNEntity>> GetAllAsync()
    {
        return await _context.GRNs.ToListAsync();
    }
}