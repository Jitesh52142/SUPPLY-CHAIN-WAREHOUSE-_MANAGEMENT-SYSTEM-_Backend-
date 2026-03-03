using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.InventoryMovement.Entities;

public class StockLedger : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }

    // IN / OUT / TRANSFER / ADJUSTMENT
    public string MovementType { get; set; } = default!;

    public string ReferenceNumber { get; set; } = default!;
}