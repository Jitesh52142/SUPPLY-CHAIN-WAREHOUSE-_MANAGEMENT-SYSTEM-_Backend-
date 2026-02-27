namespace warehouse_management_system.Modules.GRN.Entities;

public class GRNLine
{
    public Guid Id { get; set; }

    public Guid GRNId { get; set; }
    public Guid ItemId { get; set; }

    public decimal QuantityReceived { get; set; }
}