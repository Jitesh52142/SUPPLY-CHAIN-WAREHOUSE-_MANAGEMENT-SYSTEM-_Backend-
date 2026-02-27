using warehouse_management_system.Database;
using warehouse_management_system.Modules.InventoryMovement.DTOs;
using warehouse_management_system.Modules.InventoryMovement.Entities;
using warehouse_management_system.Modules.InventoryMovement.Interfaces;

namespace warehouse_management_system.Modules.InventoryMovement.Services;

public class StockMovementService
    : IStockMovementService
{
    private readonly ApplicationDbContext _context;

    public StockMovementService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateMovementAsync(
        CreateStockMovementDto dto)
    {
        var ledger = new StockLedger
        {
            ItemId = dto.ItemId,
            WarehouseId = dto.WarehouseId,
            Quantity = dto.Quantity,
            MovementType = dto.MovementType,
            ReferenceNumber = dto.ReferenceNumber
        };

        await _context.StockLedgers.AddAsync(ledger);
        await _context.SaveChangesAsync();

        return ledger.Id;
    }

    public async Task<IEnumerable<StockLedger>>
        GetLedgerAsync()
        => _context.StockLedgers.ToList();
}