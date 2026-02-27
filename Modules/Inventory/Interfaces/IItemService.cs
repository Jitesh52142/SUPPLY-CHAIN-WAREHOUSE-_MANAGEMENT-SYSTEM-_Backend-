using warehouse_management_system.Modules.Inventory.DTOs;
using warehouse_management_system.Modules.Inventory.Entities;
namespace warehouse_management_system.Modules.Inventory.Interfaces;
public interface IItemService
{
    Task<Guid> CreateAsync(CreateItemDto dto);
    Task<IEnumerable<Item>> GetAllAsync();
    Task<Item?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateItemDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Item>> SearchAsync(string keyword);
    Task<IEnumerable<Item>> GetPagedAsync(int page, int size);
}