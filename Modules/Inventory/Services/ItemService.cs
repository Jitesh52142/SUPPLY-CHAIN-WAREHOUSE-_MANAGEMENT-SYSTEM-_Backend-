using warehouse_management_system.Infrastructure.Persistence;
using warehouse_management_system.Modules.Inventory.DTOs;
using warehouse_management_system.Modules.Inventory.Entities;
using warehouse_management_system.Modules.Inventory.Interfaces;
namespace warehouse_management_system.Modules.Inventory.Services;
public class ItemService : IItemService
{
    private readonly IUnitOfWork _uow;

    public ItemService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Guid> CreateAsync(CreateItemDto dto)
    {
        var item = new Item
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Unit = dto.Unit,
            MinimumStockLevel = dto.MinimumStockLevel,
            MaximumStockLevel = dto.MaximumStockLevel
        };

        await _uow.Items.AddAsync(item);
        await _uow.SaveChangesAsync();

        return item.Id;
    }

    public async Task<IEnumerable<Item>> GetAllAsync()
        => await _uow.Items.GetAllAsync();

    public async Task<Item?> GetByIdAsync(Guid id)
        => await _uow.Items.GetByIdAsync(id);

    public async Task<bool> UpdateAsync(Guid id, UpdateItemDto dto)
    {
        var item = await _uow.Items.GetByIdAsync(id);
        if (item == null) return false;

        item.Name = dto.Name;
        item.Unit = dto.Unit;
        item.MinimumStockLevel = dto.MinimumStockLevel;
        item.MaximumStockLevel = dto.MaximumStockLevel;

        _uow.Items.Update(item);
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _uow.Items.GetByIdAsync(id);
        if (item == null) return false;

        _uow.Items.Delete(item);
        await _uow.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Item>> SearchAsync(string keyword)
        => await _uow.Items.FindAsync(x =>
            x.Name.Contains(keyword));

    public async Task<IEnumerable<Item>> GetPagedAsync(int page, int size)
    {
        var data = await _uow.Items.GetAllAsync();
        return data.Skip((page - 1) * size).Take(size);
    }
}