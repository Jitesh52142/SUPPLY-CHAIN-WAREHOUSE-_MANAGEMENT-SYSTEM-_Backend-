using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Warehouse.Entities;

public class WarehouseEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}