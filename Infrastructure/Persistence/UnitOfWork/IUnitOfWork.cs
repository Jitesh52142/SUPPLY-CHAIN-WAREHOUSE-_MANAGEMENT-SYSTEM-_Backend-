using warehouse_management_system.Modules.Inventory.Entities;
using warehouse_management_system.Modules.Vendors.Entities;
using warehouse_management_system.Modules.Warehouse.Entities;
using warehouse_management_system.Infrastructure.Persistence.Repository;

namespace warehouse_management_system.Infrastructure.Persistence;

public interface IUnitOfWork
{
    IRepository<Item> Items { get; }
    IRepository<WarehouseEntity> Warehouses { get; }
    IRepository<Vendor> Vendors { get; }

    Task<int> SaveChangesAsync();
}