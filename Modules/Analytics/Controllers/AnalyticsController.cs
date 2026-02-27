using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Analytics.Interfaces;

namespace warehouse_management_system.Modules.Analytics.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _service;

    public AnalyticsController(IAnalyticsService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
        => Ok(await _service.GetDashboardSummaryAsync());

    [HttpGet("stock-summary")]
    public async Task<IActionResult> StockSummary()
        => Ok(await _service.GetStockSummaryAsync());

    [HttpGet("movement/{itemId}")]
    public async Task<IActionResult> Movement(Guid itemId)
        => Ok(await _service.GetMovementHistoryAsync(itemId));

    [HttpGet("vendor-purchase")]
    public async Task<IActionResult> VendorPurchase()
        => Ok(await _service.GetVendorPurchaseAnalyticsAsync());

    [HttpGet("low-stock")]
    public async Task<IActionResult> LowStock()
        => Ok(await _service.GetLowStockReportAsync());
}