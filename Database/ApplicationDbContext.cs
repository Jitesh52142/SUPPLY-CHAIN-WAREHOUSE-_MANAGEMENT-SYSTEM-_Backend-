using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Modules.Audit.Entities;
using warehouse_management_system.Modules.GRN.Entities;
using warehouse_management_system.Modules.Inventory.Entities;
using warehouse_management_system.Modules.InventoryMovement.Entities;
using warehouse_management_system.Modules.PurchaseOrders.Entities;
using warehouse_management_system.Modules.Vendors.Entities;
using warehouse_management_system.Modules.Warehouse.Entities;
using warehouse_management_system.Modules.StockIssue.Entities;
using warehouse_management_system.Modules.WarehouseTransfer.Entities;
using warehouse_management_system.Modules.ROL.Entities;
using warehouse_management_system.Modules.Auth.Entities;
using warehouse_management_system.Modules.Approvals.Entities;
using warehouse_management_system.Modules.Notifications.Entities;

namespace warehouse_management_system.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // =========================
    // MASTER TABLES
    // =========================
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<WarehouseEntity> Warehouses => Set<WarehouseEntity>();

    // =========================
    // BATCH TRACKING
    // =========================
    public DbSet<Batch> Batches => Set<Batch>();

    // =========================
    // PURCHASE ORDERS
    // =========================
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();

    // =========================
    // GRN
    // =========================
    public DbSet<GRNEntity> GRNs => Set<GRNEntity>();
    public DbSet<GRNLine> GRNLines => Set<GRNLine>();

    // =========================
    // STOCK MOVEMENTS
    // =========================
    public DbSet<StockLedger> StockLedgers => Set<StockLedger>();
    public DbSet<StockIssueEntity> StockIssues => Set<StockIssueEntity>();
    public DbSet<WarehouseTransferEntity> WarehouseTransfers => Set<WarehouseTransferEntity>();

    // =========================
    // ROL
    // =========================
    public DbSet<ReorderRequest> ReorderRequests => Set<ReorderRequest>();

    // =========================
    // AUTH & WORKFLOW
    // =========================
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<Notification> Notifications => Set<Notification>();

    // =========================
    // AUDIT
    // =========================
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================
        // 🔥 CONCURRENCY CONTROL
        // =========================
        modelBuilder.Entity<AppUser>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Item>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<WarehouseEntity>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Vendor>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<PurchaseOrder>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Batch>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<AuditLog>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<Notification>().Property(x => x.RowVersion).IsRowVersion();
        modelBuilder.Entity<ApprovalRequest>().Property(x => x.RowVersion).IsRowVersion();
        // ADD THIS
        modelBuilder.Entity<GRNEntity>()
            .Property(x => x.RowVersion)
            .IsRowVersion();


        // =========================
        // 🔥 SOFT DELETE FILTERS
        // =========================
        modelBuilder.Entity<Item>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<WarehouseEntity>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Vendor>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<PurchaseOrder>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Batch>().HasQueryFilter(x => !x.IsDeleted);

        // =========================
        // 🔥 BATCH RELATIONSHIP
        // =========================
        modelBuilder.Entity<Batch>()
            .HasOne(b => b.Item)
            .WithMany()
            .HasForeignKey(b => b.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // =========================
        // 🔥 DECIMAL PRECISION STANDARDIZATION
        // =========================

        // Item
        modelBuilder.Entity<Item>()
            .Property(x => x.MinimumStockLevel)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Item>()
            .Property(x => x.MaximumStockLevel)
            .HasPrecision(18, 4);

        // Batch
        modelBuilder.Entity<Batch>()
            .Property(x => x.AvailableQuantity)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Batch>()
            .Property(x => x.QuantityReceived)
            .HasPrecision(18, 4);

        // GRN
        modelBuilder.Entity<GRNLine>()
            .Property(x => x.QuantityReceived)
            .HasPrecision(18, 4);

        // Stock
        modelBuilder.Entity<StockLedger>()
            .Property(x => x.Quantity)
            .HasPrecision(18, 4);

        modelBuilder.Entity<StockIssueEntity>()
            .Property(x => x.QuantityIssued)
            .HasPrecision(18, 4);

        modelBuilder.Entity<WarehouseTransferEntity>()
            .Property(x => x.Quantity)
            .HasPrecision(18, 4);

        // Purchase
        modelBuilder.Entity<PurchaseOrderLine>()
            .Property(x => x.Quantity)
            .HasPrecision(18, 4);

        modelBuilder.Entity<PurchaseOrderLine>()
            .Property(x => x.UnitPrice)
            .HasPrecision(18, 4);

        // ROL
        modelBuilder.Entity<ReorderRequest>()
            .Property(x => x.CurrentStock)
            .HasPrecision(18, 4);

        modelBuilder.Entity<ReorderRequest>()
            .Property(x => x.MinimumLevel)
            .HasPrecision(18, 4);


    }
}