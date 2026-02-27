namespace warehouse_management_system.Modules.Audit.DTOs;

public class CreateAuditLogDto
{
    public string Module { get; set; } = default!;
    public string Action { get; set; } = default!;
    public Guid ReferenceId { get; set; }
    public string PerformedBy { get; set; } = default!;
}
