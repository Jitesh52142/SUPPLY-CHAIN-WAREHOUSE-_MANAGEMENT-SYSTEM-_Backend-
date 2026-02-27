namespace warehouse_management_system.Modules.WarehouseTransfer.DTOs;

public class CreateTransferDto
{
    public Guid ItemId { get; set; }

    public Guid SourceWarehouseId { get; set; }

    public Guid DestinationWarehouseId { get; set; }

    public decimal Quantity { get; set; }
}