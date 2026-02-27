namespace warehouse_management_system.Modules.GRN.DTOs;

public class GRNItemDto
{
    public Guid ItemId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal QuantityReceived { get; set; }

    public string BatchNumber { get; set; } = default!;

    public DateTime ExpiryDate { get; set; }
}


