using warehouse_management_system.Modules.GRN.DTOs;
using warehouse_management_system.Modules.GRN.Entities;

namespace warehouse_management_system.Modules.GRN.Interfaces;

public interface IGRNService
{
    Task<Guid> CreateGRNAsync(CreateGRNDto dto);
    Task<bool> AddItemAsync(Guid grnId, GRNItemDto dto);
    Task<IEnumerable<GRNEntity>> GetAllAsync();
}