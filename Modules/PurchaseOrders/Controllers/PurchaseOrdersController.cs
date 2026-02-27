using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.PurchaseOrders.DTOs;
using warehouse_management_system.Modules.PurchaseOrders.Interfaces;

namespace warehouse_management_system.Modules.PurchaseOrders.Controllers;

[ApiController]
[Route("api/purchase-orders")]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _service;

    public PurchaseOrdersController(IPurchaseOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePurchaseOrderDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpPost("{poId}/lines")]
    public async Task<IActionResult> AddLine(Guid poId, AddPOLineDto dto)
        => Ok(await _service.AddLineAsync(poId, dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPatch("{poId}/status")]
    public async Task<IActionResult> UpdateStatus(Guid poId,
        UpdatePOStatusDto dto)
        => Ok(await _service.UpdateStatusAsync(poId, dto.Status));
}