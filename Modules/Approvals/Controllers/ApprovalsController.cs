using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using warehouse_management_system.Modules.Approvals.DTOs;
using warehouse_management_system.Modules.Approvals.Interfaces;

namespace warehouse_management_system.Modules.Approvals.Controllers;

[ApiController]
[Route("api/approvals")]
[Authorize] // 🔒 Require authentication for all endpoints
public class ApprovalsController : ControllerBase
{
    private readonly IApprovalService _service;

    public ApprovalsController(IApprovalService service)
    {
        _service = service;
    }

    // 🔹 Create approval (Only Admin can create)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateApprovalRequestDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(result);
    }

    // 🔹 Take action (HOD / FinanceManager / MedicalDirector only)
    [HttpPost("action")]
    [Authorize(Roles = "HOD,FinanceManager,MedicalDirector")]
    public async Task<IActionResult> Action(ApprovalActionDto dto)
    {
        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userName = User.Identity?.Name;

        if (string.IsNullOrEmpty(userRole))
            return Unauthorized("Role not found in token.");

        dto.UserRole = userRole;
        dto.UserName = userName ?? "Unknown";

        var result = await _service.TakeActionAsync(dto);
        return Ok(result);
    }

    // 🔹 View pending approvals (Role-based visibility)
    [HttpGet("pending")]
    [Authorize(Roles = "HOD,FinanceManager,MedicalDirector,Admin")]
    public async Task<IActionResult> Pending()
    {
        var result = await _service.GetPendingAsync();
        return Ok(result);
    }
}