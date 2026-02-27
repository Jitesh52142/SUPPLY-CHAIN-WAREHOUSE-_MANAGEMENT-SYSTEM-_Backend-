using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Audit.Interfaces;

namespace warehouse_management_system.Modules.Audit.Controllers;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditService _service;

    public AuditController(IAuditService service)
    {
        _service = service;
    }

    [HttpGet("reference/{referenceId}")]
    public async Task<IActionResult> GetByReference(Guid referenceId)
    {
        var logs = await _service.GetByReferenceAsync(referenceId);
        return Ok(logs);
    }

    [HttpGet("module/{module}")]
    public async Task<IActionResult> GetByModule(string module)
    {
        var logs = await _service.GetByModuleAsync(module);
        return Ok(logs);
    }
}
