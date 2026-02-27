using warehouse_management_system.Database;
using warehouse_management_system.Infrastructure.Persistence.Repository;
using warehouse_management_system.Modules.Inventory.Entities;
using warehouse_management_system.Modules.Vendors.Entities;
using warehouse_management_system.Modules.Warehouse.Entities;

namespace warehouse_management_system.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRepository<Item> Items { get; }
    public IRepository<WarehouseEntity> Warehouses { get; }
    public IRepository<Vendor> Vendors { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Items = new Repository<Item>(_context);
        Warehouses = new Repository<WarehouseEntity>(_context);
        Vendors = new Repository<Vendor>(_context);
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();
}