using warehouse_management_system.Shared.Common;

namespace warehouse_management_system.Modules.WarehouseTransfer.Entities;

public class WarehouseTransferEntity : BaseEntity
{
    public Guid ItemId { get; set; }

    public Guid SourceWarehouseId { get; set; }

    public Guid DestinationWarehouseId { get; set; }

    public decimal Quantity { get; set; }
}