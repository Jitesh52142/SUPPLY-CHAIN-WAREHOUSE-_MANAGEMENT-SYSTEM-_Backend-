using warehouse_management_system.Modules.InventoryMovement.DTOs;
using warehouse_management_system.Modules.InventoryMovement.Entities;

namespace warehouse_management_system.Modules.InventoryMovement.Interfaces;

public interface IStockMovementService
{
    Task<Guid> CreateMovementAsync(
        CreateStockMovementDto dto);

    Task<IEnumerable<StockLedger>> GetLedgerAsync();
}