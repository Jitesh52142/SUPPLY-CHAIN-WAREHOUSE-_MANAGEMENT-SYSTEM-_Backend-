namespace warehouse_management_system.Modules.Approvals.DTOs;

public class ApprovalActionDto
{
    public Guid ApprovalId { get; set; }
    public string Action { get; set; } = default!;
    // "Approved" or "Rejected"

    public string UserRole { get; set; } = default!;
    public string UserName { get; set; } = default!;
}