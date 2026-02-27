using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.Audit.DTOs;
using warehouse_management_system.Modules.Audit.Entities;
using warehouse_management_system.Modules.Audit.Interfaces;

namespace warehouse_management_system.Modules.Audit.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateAsync(CreateAuditLogDto dto)
    {
        var auditLog = new AuditLog
        {
            Module = dto.Module,
            Action = dto.Action,
            ReferenceId = dto.ReferenceId,
            PerformedBy = dto.PerformedBy
        };

        await _context.AuditLogs.AddAsync(auditLog);
        await _context.SaveChangesAsync();

        return auditLog.Id;
    }

    public async Task<IEnumerable<AuditLog>> GetByReferenceAsync(Guid referenceId)
    {
        return await _context.AuditLogs
            .Where(x => x.ReferenceId == referenceId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<AuditLog>> GetByModuleAsync(string module)
    {
        return await _context.AuditLogs
            .Where(x => x.Module == module)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
