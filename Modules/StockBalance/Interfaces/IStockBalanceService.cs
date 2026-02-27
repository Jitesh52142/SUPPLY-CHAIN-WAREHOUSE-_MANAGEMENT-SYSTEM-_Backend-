using warehouse_management_system.Modules.StockBalance.DTOs;

namespace warehouse_management_system.Modules.StockBalance.Interfaces;

public interface IStockBalanceService
{
    Task<StockBalanceDto> GetItemStockAsync(
        Guid itemId,
        Guid warehouseId);

    Task<IEnumerable<StockBalanceDto>> GetAllStockAsync();
}