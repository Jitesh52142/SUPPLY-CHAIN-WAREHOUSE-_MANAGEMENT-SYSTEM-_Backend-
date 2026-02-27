using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Audit.Entities;

public class AuditLog : BaseEntity
{
    public string Module { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid ReferenceId { get; set; }
    public string PerformedBy { get; set; } = default!;
}
