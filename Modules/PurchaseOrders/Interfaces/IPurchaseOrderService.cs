using warehouse_management_system.Modules.PurchaseOrders.DTOs;
using warehouse_management_system.Modules.PurchaseOrders.Entities;

namespace warehouse_management_system.Modules.PurchaseOrders.Interfaces;

public interface IPurchaseOrderService
{
    Task<Guid> CreateAsync(CreatePurchaseOrderDto dto);
    Task<bool> AddLineAsync(Guid poId, AddPOLineDto dto);
    Task<IEnumerable<PurchaseOrder>> GetAllAsync();
    Task<bool> UpdateStatusAsync(Guid poId, string status);
}