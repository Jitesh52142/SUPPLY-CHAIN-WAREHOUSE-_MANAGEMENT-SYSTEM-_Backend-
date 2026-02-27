using warehouse_management_system.Modules.StockIssue.DTOs;

namespace warehouse_management_system.Modules.StockIssue.Interfaces;

public interface IStockIssueService
{
    Task<Guid> IssueAsync(CreateStockIssueDto dto);
}