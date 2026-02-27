public class UpdateItemDto
{
    public string Name { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public decimal MinimumStockLevel { get; set; }
    public decimal MaximumStockLevel { get; set; }
}