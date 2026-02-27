using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.GRN.DTOs;
using warehouse_management_system.Modules.GRN.Interfaces;

namespace warehouse_management_system.Modules.GRN.Controllers;

[ApiController]
[Route("api/grn")]
public class GRNController : ControllerBase
{
    private readonly IGRNService _service;

    public GRNController(IGRNService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGRNDto dto)
        => Ok(await _service.CreateGRNAsync(dto));

    [HttpPost("{grnId}/items")]
    public async Task<IActionResult> AddItem(
        Guid grnId,
        GRNItemDto dto)
        => Ok(await _service.AddItemAsync(grnId, dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());
}