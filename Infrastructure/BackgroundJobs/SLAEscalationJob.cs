using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Interfaces;

namespace warehouse_management_system.Infrastructure.BackgroundJobs;

public class SLAEscalationJob
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public SLAEscalationJob(
        ApplicationDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task CheckAndEscalateAsync()
    {
        var now = DateTime.UtcNow;

        var overdueApprovals = await _context.ApprovalRequests
            .Where(a =>
                a.Status == "Pending" &&
                a.SLADeadline != null &&
                a.SLADeadline < now)
            .ToListAsync();

        foreach (var approval in overdueApprovals)
        {
            if (approval.CurrentLevel == 1)
            {
                approval.CurrentLevel = 2;
                approval.CurrentRole = "FinanceManager";
                approval.SLADeadline = now.AddHours(4);
            }
            else if (approval.CurrentLevel == 2)
            {
                approval.CurrentLevel = 3;
                approval.CurrentRole = "MedicalDirector";
                approval.SLADeadline = now.AddHours(4);
            }
            else
            {
                // Final level expired — mark rejected automatically
                approval.Status = "Rejected";
                approval.RejectionReason = "Auto rejected due to SLA breach.";
                approval.ApprovedAt = now;
                continue;
            }

            await _notificationService.CreateAsync(
                new CreateNotificationDto
                {
                    Title = "SLA Escalation",
                    Message =
                        $"Approval for {approval.ModuleName} escalated to {approval.CurrentRole}",
                    TargetRole = approval.CurrentRole
                });
        }

        await _context.SaveChangesAsync();
    }
}