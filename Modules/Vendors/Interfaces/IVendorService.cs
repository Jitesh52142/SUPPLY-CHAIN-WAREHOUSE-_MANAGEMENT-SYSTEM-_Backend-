using warehouse_management_system.Modules.Vendors.DTOs;
using warehouse_management_system.Modules.Vendors.Entities;

namespace warehouse_management_system.Modules.Vendors.Interfaces;

public interface IVendorService
{
    Task<Guid> CreateAsync(CreateVendorDto dto);
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<Vendor?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateVendorDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ToggleActiveAsync(Guid id);
}