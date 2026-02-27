using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Entities;

namespace warehouse_management_system.Modules.Notifications.Interfaces;

public interface INotificationService
{
    Task<Guid> CreateAsync(CreateNotificationDto dto);

    Task<IEnumerable<Notification>> GetByRoleAsync(string role);

    Task<bool> MarkAsReadAsync(Guid id);
}