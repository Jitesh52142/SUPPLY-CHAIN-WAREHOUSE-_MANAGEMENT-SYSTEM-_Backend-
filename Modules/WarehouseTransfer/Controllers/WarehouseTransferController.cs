using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.WarehouseTransfer.DTOs;
using warehouse_management_system.Modules.WarehouseTransfer.Interfaces;

namespace warehouse_management_system.Modules.WarehouseTransfer.Controllers;

[ApiController]
[Route("api/warehouse-transfer")]
public class WarehouseTransferController : ControllerBase
{
    private readonly IWarehouseTransferService _service;

    public WarehouseTransferController(
        IWarehouseTransferService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(
        CreateTransferDto dto)
        => Ok(await _service.TransferAsync(dto));
}