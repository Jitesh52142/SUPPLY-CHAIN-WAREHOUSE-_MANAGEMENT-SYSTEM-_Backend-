using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Approvals.Entities;

public class ApprovalRequest : BaseEntity
{
    // 🔹 Module requesting approval (PurchaseOrder / GRN / ROL)
    public string ModuleName { get; set; } = default!;

    // 🔹 Id of PO / GRN / etc.
    public Guid ReferenceId { get; set; }

    // 🔹 Workflow Status
    // Pending / Approved / Rejected
    public string Status { get; set; } = "Pending";

    // 🔹 Multi-Level Control
    public int CurrentLevel { get; set; } = 1;
    public int TotalLevels { get; set; } = 3;

    // 🔹 Role currently responsible
    // HOD → FinanceManager → MedicalDirector
    public string CurrentRole { get; set; } = "HOD";

    // 🔹 SLA tracking
    public DateTime? SLADeadline { get; set; }

    // 🔹 Final Approval Metadata
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // 🔹 Optional rejection reason
    public string? RejectionReason { get; set; }

}