using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Vendors.Entities;

public class Vendor : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string GSTNumber { get; set; } = default!;
    public bool IsActive { get; set; } = true;
}