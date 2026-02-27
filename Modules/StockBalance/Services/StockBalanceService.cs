using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.StockBalance.DTOs;
using warehouse_management_system.Modules.StockBalance.Interfaces;

namespace warehouse_management_system.Modules.StockBalance.Services;

public class StockBalanceService : IStockBalanceService
{
    private readonly ApplicationDbContext _context;

    public StockBalanceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StockBalanceDto> GetItemStockAsync(
        Guid itemId,
        Guid warehouseId)
    {
        var stock = await _context.Batches
            .Where(b => b.ItemId == itemId &&
                        b.WarehouseId == warehouseId)
            .SumAsync(b => (decimal?)b.AvailableQuantity) ?? 0;

        return new StockBalanceDto
        {
            ItemId = itemId,
            WarehouseId = warehouseId,
            CurrentStock = stock
        };
    }

    public async Task<IEnumerable<StockBalanceDto>>
        GetAllStockAsync()
    {
        var grouped = await _context.Batches
            .GroupBy(b => new { b.ItemId, b.WarehouseId })
            .Select(g => new StockBalanceDto
            {
                ItemId = g.Key.ItemId,
                WarehouseId = g.Key.WarehouseId,
                CurrentStock = g.Sum(x => x.AvailableQuantity)
            })
            .ToListAsync();

        return grouped;
    }
}