namespace warehouse_management_system.Shared.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;

    // Concurrency Token (SQL Server rowversion)
    public byte[] RowVersion { get; set; } = default!;
}