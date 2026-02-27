using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Inventory.DTOs;
using warehouse_management_system.Modules.Inventory.Interfaces;




[ApiController]
[Route("api/inventory/items")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _service;

    public ItemsController(IItemService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => Ok(await _service.GetByIdAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateItemDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _service.DeleteAsync(id));

    [HttpGet("search")]
    public async Task<IActionResult> Search(string keyword)
        => Ok(await _service.SearchAsync(keyword));

    [HttpGet("paged")]
    public async Task<IActionResult> Paged(int page = 1, int size = 10)
        => Ok(await _service.GetPagedAsync(page, size));
}