using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Interfaces;

namespace warehouse_management_system.Modules.Approvals.Services;

public class ApprovalEscalationService
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ApprovalEscalationService(
        ApplicationDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task EscalatePendingApprovals()
    {
        var threshold = DateTime.UtcNow.AddHours(-24);

        var pending = await _context.ApprovalRequests
            .Where(x => x.Status == "Pending"
                        && x.CreatedAt <= threshold)
            .ToListAsync();

        foreach (var approval in pending)
        {
            await _notificationService.CreateAsync(
                new CreateNotificationDto
                {
                    Title = "Approval Escalation",
                    Message = $"Approval for {approval.ModuleName} is pending for more than 24 hours. Reference: {approval.ReferenceId}",
                    TargetRole = "Admin"
                });
        }
    }
}
