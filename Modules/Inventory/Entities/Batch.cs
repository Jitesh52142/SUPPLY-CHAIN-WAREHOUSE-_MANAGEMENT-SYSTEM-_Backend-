using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Inventory.Entities;

public class Batch : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }

    public string BatchNumber { get; set; } = default!;

    public DateTime ExpiryDate { get; set; }

    // Total quantity received in this batch
    public decimal QuantityReceived { get; set; }

    // Remaining quantity available (for FIFO deduction)
    public decimal AvailableQuantity { get; set; }

    // Navigation
    public Item? Item { get; set; }
}