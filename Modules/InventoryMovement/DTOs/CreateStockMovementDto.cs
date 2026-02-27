namespace warehouse_management_system.Modules.InventoryMovement.DTOs;

public class CreateStockMovementDto
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }

    public string MovementType { get; set; } = default!;
    public string ReferenceNumber { get; set; } = default!;
}