using warehouse_management_system.Modules.Approvals.DTOs;
using warehouse_management_system.Modules.Approvals.Entities;

namespace warehouse_management_system.Modules.Approvals.Interfaces;

public interface IApprovalService
{
    Task<Guid> CreateAsync(CreateApprovalRequestDto dto);

    Task<bool> TakeActionAsync(ApprovalActionDto dto);

    Task<IEnumerable<ApprovalRequest>> GetPendingAsync();
}                                   