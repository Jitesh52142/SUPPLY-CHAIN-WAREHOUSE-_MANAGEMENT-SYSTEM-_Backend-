using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using warehouse_management_system.Infrastructure.Persistence;
using warehouse_management_system.Modules.Inventory.Interfaces;
using warehouse_management_system.Modules.Inventory.Services;
using warehouse_management_system.Modules.Warehouse.Interfaces;
using warehouse_management_system.Modules.Warehouse.Services;
using warehouse_management_system.Modules.Vendors.Interfaces;
using warehouse_management_system.Modules.Vendors.Services;
using warehouse_management_system.Modules.StockIssue.Interfaces;
using warehouse_management_system.Modules.StockIssue.Services;
using warehouse_management_system.Modules.WarehouseTransfer.Interfaces;
using warehouse_management_system.Modules.WarehouseTransfer.Services;
using warehouse_management_system.Modules.PurchaseOrders.Interfaces;
using warehouse_management_system.Modules.PurchaseOrders.Services;
using warehouse_management_system.Modules.GRN.Interfaces;
using warehouse_management_system.Modules.GRN.Services;
using warehouse_management_system.Modules.InventoryMovement.Interfaces;
using warehouse_management_system.Modules.InventoryMovement.Services;
using warehouse_management_system.Modules.ROL.Interfaces;
using warehouse_management_system.Modules.ROL.Services;
using warehouse_management_system.Modules.Auth.Interfaces;
using warehouse_management_system.Modules.Auth.Services;
using warehouse_management_system.Modules.Approvals.Interfaces;
using warehouse_management_system.Modules.Approvals.Services;
using warehouse_management_system.Modules.Notifications.Interfaces;
using warehouse_management_system.Modules.Notifications.Services;
using warehouse_management_system.Modules.StockBalance.Interfaces;
using warehouse_management_system.Modules.StockBalance.Services;
using warehouse_management_system.Modules.Audit.Interfaces;
using warehouse_management_system.Modules.Audit.Services;
using warehouse_management_system.Modules.Analytics.Interfaces;
using warehouse_management_system.Modules.Analytics.Services;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Dashboard;

namespace warehouse_management_system.Config;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(
        this IServiceCollection services)
    {
        // Unit Of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Inventory Services
        services.AddScoped<IItemService, ItemService>();

        services.AddScoped<IWarehouseService, WarehouseService>();

        services.AddScoped<IVendorService, VendorService>();

        services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

        services.AddScoped<IGRNService, GRNService>();

        services.AddScoped<IStockMovementService, StockMovementService>();

        services.AddScoped<IStockIssueService, StockIssueService>();

        services.AddScoped<IWarehouseTransferService, WarehouseTransferService>();

        services.AddScoped<IRolService, RolService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IApprovalService, ApprovalService>();

        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<IStockBalanceService, StockBalanceService>();

        services.AddScoped<IAuditService, AuditService>();

        services.AddScoped<IAnalyticsService, AnalyticsService>();
        // 🔥 Background Jobs
        services.AddScoped<ApprovalEscalationService>();

        return services;
    }

    public static IServiceCollection AddHangfireServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(config =>
            config.UseSqlServerStorage(
                configuration.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static WebApplication UseHangfireServices(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new AllowAllAuthorizationFilter() }
        });
        app.Services.GetRequiredService<IBackgroundJobClient>();
        
        return app;
    }

    private class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
