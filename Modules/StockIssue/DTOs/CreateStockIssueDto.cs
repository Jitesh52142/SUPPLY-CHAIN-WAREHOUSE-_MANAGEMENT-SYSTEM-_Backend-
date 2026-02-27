namespace warehouse_management_system.Modules.StockIssue.DTOs;

public class CreateStockIssueDto
{
    public Guid ItemId { get; set; }

    public Guid WarehouseId { get; set; }

    public decimal QuantityIssued { get; set; }

    public string IssueReason { get; set; } = default!;
}