namespace warehouse_management_system.Modules.Notifications.DTOs;

public class CreateNotificationDto
{
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string TargetRole { get; set; } = default!;
}