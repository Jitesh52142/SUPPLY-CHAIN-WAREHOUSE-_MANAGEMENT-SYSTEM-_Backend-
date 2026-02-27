using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Vendors.DTOs;
using warehouse_management_system.Modules.Vendors.Interfaces;

namespace warehouse_management_system.Modules.Vendors.Controllers;

[ApiController]
[Route("api/vendors")]
public class VendorsController : ControllerBase
{
    private readonly IVendorService _service;

    public VendorsController(IVendorService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVendorDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateVendorDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _service.DeleteAsync(id));

    [HttpPatch("{id}/toggle")]
    public async Task<IActionResult> Toggle(Guid id)
        => Ok(await _service.ToggleActiveAsync(id));
}