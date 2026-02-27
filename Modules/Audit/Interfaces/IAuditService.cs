using warehouse_management_system.Modules.Audit.DTOs;
using warehouse_management_system.Modules.Audit.Entities;

namespace warehouse_management_system.Modules.Audit.Interfaces;

public interface IAuditService
{
    Task<Guid> CreateAsync(CreateAuditLogDto dto);
    Task<IEnumerable<AuditLog>> GetByReferenceAsync(Guid referenceId);
    Task<IEnumerable<AuditLog>> GetByModuleAsync(string module);
}
