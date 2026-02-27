using Microsoft.AspNetCore.Mvc;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Interfaces;

namespace warehouse_management_system.Modules.Notifications.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateNotificationDto dto)
        => Ok(await _service.CreateAsync(dto));

    [HttpGet("{role}")]
    public async Task<IActionResult> GetByRole(string role)
        => Ok(await _service.GetByRoleAsync(role));

    [HttpPatch("{id}")]
    public async Task<IActionResult> MarkAsRead(Guid id)
        => Ok(await _service.MarkAsReadAsync(id));
}