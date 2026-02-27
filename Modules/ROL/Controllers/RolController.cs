using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.ROL.Interfaces;

namespace warehouse_management_system.Modules.ROL.Controllers;

[ApiController]
[Route("api/rol")]
public class RolController : ControllerBase
{
    private readonly IRolService _service;

    public RolController(IRolService service)
    {
        _service = service;
    }

    [HttpPost("check")]
    public async Task<IActionResult> Check()
    {
        await _service.CheckStockAsync();
        return Ok("ROL Checked");
    }
}
