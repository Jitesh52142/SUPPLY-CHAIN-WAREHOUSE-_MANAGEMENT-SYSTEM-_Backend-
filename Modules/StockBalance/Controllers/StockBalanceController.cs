using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.StockBalance.Interfaces;

namespace warehouse_management_system.Modules.StockBalance.Controllers;

[ApiController]
[Route("api/stock-balance")]
public class StockBalanceController : ControllerBase
{
    private readonly IStockBalanceService _service;

    public StockBalanceController(
        IStockBalanceService service)
    {
        _service = service;
    }

    [HttpGet("{itemId}/{warehouseId}")]
    public async Task<IActionResult> GetSingle(
        Guid itemId,
        Guid warehouseId)
        => Ok(await _service
            .GetItemStockAsync(itemId, warehouseId));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllStockAsync());
}