namespace warehouse_management_system.Modules.Warehouse.DTOs;

public class WarehouseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
}