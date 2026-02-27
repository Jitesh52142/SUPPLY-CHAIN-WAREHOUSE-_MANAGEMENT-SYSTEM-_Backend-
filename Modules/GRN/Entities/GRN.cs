using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.GRN.Entities;

public class GRNEntity : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }

    public string GRNNumber { get; set; } = default!;
    public DateTime ReceivedDate { get; set; }
        = DateTime.UtcNow;

    public ICollection<GRNLine> Lines { get; set; }
        = new List<GRNLine>();
}