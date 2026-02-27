using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.StockIssue.DTOs;
using warehouse_management_system.Modules.StockIssue.Interfaces;

namespace warehouse_management_system.Modules.StockIssue.Controllers;

[ApiController]
[Route("api/stock-issue")]
public class StockIssueController : ControllerBase
{
    private readonly IStockIssueService _service;

    public StockIssueController(IStockIssueService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Issue(CreateStockIssueDto dto)
    {
        var result = await _service.IssueAsync(dto);
        return Ok(result);
    }
}