using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.ROL.Entities;

public class ReorderRequest : BaseEntity
{
    public Guid ItemId { get; set; }

    public decimal CurrentStock { get; set; }

    public decimal MinimumLevel { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔥 Link to Auto Generated PO
    public Guid? PurchaseOrderId { get; set; }
}