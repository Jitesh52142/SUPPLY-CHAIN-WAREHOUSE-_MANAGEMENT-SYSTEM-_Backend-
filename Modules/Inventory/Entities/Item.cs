using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.Inventory.Entities;

public class Item : BaseEntity
{
    public string Name { get; set; } = default!;
    public string SKU { get; set; } = default!;
    public string Unit { get; set; } = default!;

    public decimal MinimumStockLevel { get; set; }
    public decimal MaximumStockLevel { get; set; }

    public int LeadTimeDays { get; set; } = 7;
}