using warehouse_management_system.Infrastructure.Persistence;
using warehouse_management_system.Modules.Vendors.DTOs;
using warehouse_management_system.Modules.Vendors.Entities;
using warehouse_management_system.Modules.Vendors.Interfaces;

namespace warehouse_management_system.Modules.Vendors.Services;

public class VendorService : IVendorService
{
    private readonly IUnitOfWork _uow;

    public VendorService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Guid> CreateAsync(CreateVendorDto dto)
    {
        var vendor = new Vendor
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            GSTNumber = dto.GSTNumber
        };

        await _uow.Vendors.AddAsync(vendor);
        await _uow.SaveChangesAsync();

        return vendor.Id;
    }

    public async Task<IEnumerable<Vendor>> GetAllAsync()
        => await _uow.Vendors.GetAllAsync();

    public async Task<Vendor?> GetByIdAsync(Guid id)
        => await _uow.Vendors.GetByIdAsync(id);

    public async Task<bool> UpdateAsync(Guid id, UpdateVendorDto dto)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(id);
        if (vendor == null) return false;

        vendor.Name = dto.Name;
        vendor.Phone = dto.Phone;
        vendor.Address = dto.Address;

        _uow.Vendors.Update(vendor);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(id);
        if (vendor == null) return false;

        _uow.Vendors.Delete(vendor);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var vendor = await _uow.Vendors.GetByIdAsync(id);
        if (vendor == null) return false;

        vendor.IsActive = !vendor.IsActive;
        await _uow.SaveChangesAsync();
        return true;
    }
}