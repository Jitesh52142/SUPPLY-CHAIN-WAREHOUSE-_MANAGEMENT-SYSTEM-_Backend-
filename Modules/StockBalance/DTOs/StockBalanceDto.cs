namespace warehouse_management_system.Modules.StockBalance.DTOs;

public class StockBalanceDto
{
    public Guid ItemId { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal CurrentStock { get; set; }
}