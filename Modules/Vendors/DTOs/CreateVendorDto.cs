namespace warehouse_management_system.Modules.Vendors.DTOs;

public class CreateVendorDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string GSTNumber { get; set; } = default!;
}