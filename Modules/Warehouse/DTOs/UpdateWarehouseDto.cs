namespace warehouse_management_system.Modules.Warehouse.DTOs;

public class UpdateWarehouseDto
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
}