public class ItemResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public string Unit { get; set; } = default!;
}