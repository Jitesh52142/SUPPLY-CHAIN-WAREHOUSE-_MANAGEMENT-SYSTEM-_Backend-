using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Notifications.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = default!;

    public string Message { get; set; } = default!;

    public string TargetRole { get; set; } = default!;
    // Example: "Admin", "Manager", "User"

    public bool IsRead { get; set; } = false;

    public byte[] RowVersion { get; set; } = default!;
}