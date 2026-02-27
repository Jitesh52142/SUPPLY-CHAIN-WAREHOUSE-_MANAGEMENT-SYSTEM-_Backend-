namespace warehouse_management_system.Modules.PurchaseOrders.DTOs;

public class AddPOLineDto
{
    public Guid ItemId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}