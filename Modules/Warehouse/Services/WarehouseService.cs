using warehouse_management_system.Infrastructure.Persistence;
using warehouse_management_system.Modules.Warehouse.DTOs;
using warehouse_management_system.Modules.Warehouse.Entities;
using warehouse_management_system.Modules.Warehouse.Interfaces;

namespace warehouse_management_system.Modules.Warehouse.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IUnitOfWork _uow;

    public WarehouseService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Guid> CreateAsync(CreateWarehouseDto dto)
    {
        var entity = new WarehouseEntity
        {
            Name = dto.Name,
            Code = dto.Code,
            Address = dto.Address,
            City = dto.City
        };

        await _uow.Warehouses.AddAsync(entity);
        await _uow.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<IEnumerable<WarehouseEntity>> GetAllAsync()
        => await _uow.Warehouses.GetAllAsync();

    public async Task<WarehouseEntity?> GetByIdAsync(Guid id)
        => await _uow.Warehouses.GetByIdAsync(id);

    public async Task<bool> UpdateAsync(Guid id, UpdateWarehouseDto dto)
    {
        var data = await _uow.Warehouses.GetByIdAsync(id);
        if (data == null) return false;

        data.Name = dto.Name;
        data.Address = dto.Address;
        data.City = dto.City;

        _uow.Warehouses.Update(data);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var data = await _uow.Warehouses.GetByIdAsync(id);
        if (data == null) return false;

        _uow.Warehouses.Delete(data);
        await _uow.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleActiveAsync(Guid id)
    {
        var data = await _uow.Warehouses.GetByIdAsync(id);
        if (data == null) return false;

        data.IsActive = !data.IsActive;
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<WarehouseEntity>> GetPagedAsync(int page, int size)
    {
        var all = await _uow.Warehouses.GetAllAsync();
        return all.Skip((page - 1) * size).Take(size);
    }
}