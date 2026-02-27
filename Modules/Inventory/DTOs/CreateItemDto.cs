namespace warehouse_management_system.Modules.Inventory.DTOs;


public class CreateItemDto
{
    public string Name { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public decimal MinimumStockLevel { get; set; }
    public decimal MaximumStockLevel { get; set; }
}