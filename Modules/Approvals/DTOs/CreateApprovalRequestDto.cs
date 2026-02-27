namespace warehouse_management_system.Modules.Approvals.DTOs;

public class CreateApprovalRequestDto
{
    public string ModuleName { get; set; } = default!;
    public Guid ReferenceId { get; set; }
}