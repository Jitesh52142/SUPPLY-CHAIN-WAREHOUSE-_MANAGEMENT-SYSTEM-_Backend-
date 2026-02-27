namespace warehouse_management_system.Modules.PurchaseOrders.DTOs;

public class CreatePurchaseOrderDto
{
    public Guid VendorId { get; set; }
    public Guid WarehouseId { get; set; }
}