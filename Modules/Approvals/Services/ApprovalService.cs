using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Approvals.DTOs;
using warehouse_management_system.Modules.Approvals.Entities;
using warehouse_management_system.Modules.Approvals.Interfaces;
using warehouse_management_system.Modules.Notifications.DTOs;
using warehouse_management_system.Modules.Notifications.Interfaces;

namespace warehouse_management_system.Modules.Approvals.Services;

public class ApprovalService : IApprovalService
{
    private readonly ApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ApprovalService(
        ApplicationDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Guid> CreateAsync(CreateApprovalRequestDto dto)
    {
        var approval = new ApprovalRequest
        {
            ModuleName = dto.ModuleName,
            ReferenceId = dto.ReferenceId,
            Status = "Pending",
            CurrentLevel = 1,
            TotalLevels = 3,
            CurrentRole = "HOD",
            SLADeadline = DateTime.UtcNow.AddHours(4)
        };

        await _context.ApprovalRequests.AddAsync(approval);
        await _context.SaveChangesAsync();

        return approval.Id;
    }

    public async Task<bool> TakeActionAsync(ApprovalActionDto dto)
    {
        var approval = await _context.ApprovalRequests
            .FirstOrDefaultAsync(x => x.Id == dto.ApprovalId);

        if (approval == null)
            return false;

        if (approval.Status != "Pending")
            throw new InvalidOperationException("Approval already completed.");

        // 🔒 Validate correct role for this level
        if (dto.UserRole != approval.CurrentRole)
            throw new UnauthorizedAccessException(
                $"Only {approval.CurrentRole} can approve at this level.");

        if (dto.Action == "Rejected")
        {
            approval.Status = "Rejected";
            approval.ApprovedAt = DateTime.UtcNow;
            approval.ApprovedBy = dto.UserName;

            await _context.SaveChangesAsync();
            return true;
        }

        if (dto.Action == "Approved")
        {
            // 🔹 LEVEL 1 → LEVEL 2
            if (approval.CurrentLevel == 1)
            {
                approval.CurrentLevel = 2;
                approval.CurrentRole = "FinanceManager";
                approval.SLADeadline = DateTime.UtcNow.AddHours(4);
            }
            // 🔹 LEVEL 2 → LEVEL 3
            else if (approval.CurrentLevel == 2)
            {
                approval.CurrentLevel = 3;
                approval.CurrentRole = "MedicalDirector";
                approval.SLADeadline = DateTime.UtcNow.AddHours(4);
            }
            // 🔹 FINAL APPROVAL
            else
            {
                approval.Status = "Approved";
                approval.ApprovedAt = DateTime.UtcNow;
                approval.ApprovedBy = dto.UserName;

                if (approval.ModuleName == "PurchaseOrder")
                {
                    var po = await _context.PurchaseOrders
                        .FindAsync(approval.ReferenceId);

                    if (po != null)
                        po.Status = "Approved";
                }
            }

            await _context.SaveChangesAsync();

            await _notificationService.CreateAsync(
                new CreateNotificationDto
                {
                    Title = "Approval Update",
                    Message =
                        $"Approval moved to Level {approval.CurrentLevel} ({approval.CurrentRole})",
                    TargetRole = approval.CurrentRole
                });

            return true;
        }

        return false;
    }

    public async Task<IEnumerable<ApprovalRequest>> GetPendingAsync()
    {
        return await _context.ApprovalRequests
            .Where(x => x.Status == "Pending")
            .ToListAsync();
    }
}