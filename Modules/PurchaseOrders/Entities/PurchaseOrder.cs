using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.PurchaseOrders.Entities;

public class PurchaseOrder : BaseEntity
{
    public Guid VendorId { get; set; }
    public Guid WarehouseId { get; set; }

    public string PONumber { get; set; } = default!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "PendingApproval";

    public ICollection<PurchaseOrderLine> Lines { get; set; }
        = new List<PurchaseOrderLine>();
}