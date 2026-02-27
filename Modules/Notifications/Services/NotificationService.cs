using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Entities;
using warehouse_management_system.Modules.Notifications.Interfaces;

namespace warehouse_management_system.Modules.Notifications.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;

    public NotificationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            TargetRole = dto.TargetRole
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        return notification.Id;
    }

    public async Task<IEnumerable<Notification>> GetByRoleAsync(string role)
    {
        return await _context.Notifications
            .Where(x => x.TargetRole == role && !x.IsRead)
            .ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(Guid id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification == null) return false;

        notification.IsRead = true;
        await _context.SaveChangesAsync();
        return true;
    }
}