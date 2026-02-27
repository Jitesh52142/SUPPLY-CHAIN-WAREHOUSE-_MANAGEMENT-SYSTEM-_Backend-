using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.InventoryMovement.DTOs;
using warehouse_management_system.Modules.InventoryMovement.Interfaces;

namespace warehouse_management_system.Modules.InventoryMovement.Controllers;

[ApiController]
[Route("api/stock-movements")]
public class StockMovementController : ControllerBase
{
    private readonly IStockMovementService _service;

    public StockMovementController(
        IStockMovementService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateStockMovementDto dto)
        => Ok(await _service.CreateMovementAsync(dto));

    [HttpGet]
    public async Task<IActionResult> Ledger()
        => Ok(await _service.GetLedgerAsync());
}