using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Warehouse.DTOs;
using warehouse_management_system.Modules.Warehouse.Interfaces;
using warehouse_management_system.Modules.Warehouse.Entities;

namespace warehouse_management_system.Modules.Warehouse.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _service;

    public WarehousesController(IWarehouseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWarehouseDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateWarehouseDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _service.DeleteAsync(id));

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> Toggle(Guid id)
        => Ok(await _service.ToggleActiveAsync(id));

    [HttpGet("paged")]
    public async Task<IActionResult> Paged(int page = 1, int size = 10)
        => Ok(await _service.GetPagedAsync(page, size));
}