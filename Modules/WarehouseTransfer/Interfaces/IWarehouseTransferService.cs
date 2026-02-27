using warehouse_management_system.Modules.WarehouseTransfer.DTOs;

namespace warehouse_management_system.Modules.WarehouseTransfer.Interfaces;

public interface IWarehouseTransferService
{
    Task<Guid> TransferAsync(CreateTransferDto dto);
}