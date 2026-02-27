using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.StockIssue.Entities;

public class StockIssueEntity : BaseEntity
{
    public Guid ItemId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal QuantityIssued { get; set; }

    public string IssueReason { get; set; } = default!;
}