using warehouse_management_system.Modules.StockBalance.DTOs;

namespace warehouse_management_system.Modules.Analytics.Interfaces;

public interface IAnalyticsService
{
    Task<object> GetDashboardSummaryAsync();

    Task<IEnumerable<object>> GetStockSummaryAsync();

    Task<IEnumerable<object>> GetMovementHistoryAsync(Guid itemId);

    Task<IEnumerable<object>> GetVendorPurchaseAnalyticsAsync();

    Task<IEnumerable<object>> GetLowStockReportAsync();
}