using warehouse_management_system.Modules.Warehouse.DTOs;
using warehouse_management_system.Modules.Warehouse.Entities;

namespace warehouse_management_system.Modules.Warehouse.Interfaces;

public interface IWarehouseService
{
    Task<Guid> CreateAsync(CreateWarehouseDto dto);
    Task<IEnumerable<WarehouseEntity>> GetAllAsync();
    Task<WarehouseEntity?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateWarehouseDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ToggleActiveAsync(Guid id);
    Task<IEnumerable<WarehouseEntity>> GetPagedAsync(int page, int size);
}