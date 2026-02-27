using Microsoft.EntityFrameworkCore;
using warehouse_management_system.Database;
using warehouse_management_system.Modules.InventoryMovement.Entities;
using warehouse_management_system.Modules.StockBalance.Interfaces;
using warehouse_management_system.Modules.StockIssue.DTOs;
using warehouse_management_system.Modules.StockIssue.Entities;
using warehouse_management_system.Modules.StockIssue.Interfaces;

namespace warehouse_management_system.Modules.StockIssue.Services;

public class StockIssueService : IStockIssueService
{
    private readonly ApplicationDbContext _context;
    private readonly IStockBalanceService _stockBalanceService;

    public StockIssueService(
        ApplicationDbContext context,
        IStockBalanceService stockBalanceService)
    {
        _context = context;
        _stockBalanceService = stockBalanceService;
    }

    public async Task<Guid> IssueAsync(CreateStockIssueDto dto)
    {
        if (dto.QuantityIssued <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 🔹 Validate current stock from batches
            var balance = await _stockBalanceService
                .GetItemStockAsync(dto.ItemId, dto.WarehouseId);

            if (balance.CurrentStock < dto.QuantityIssued)
                throw new InvalidOperationException("Insufficient stock.");

            var remainingQty = dto.QuantityIssued;

            // 🔹 Strict FIFO (Oldest Expiry First, then oldest creation)
            var batches = await _context.Batches
                .Where(b => b.ItemId == dto.ItemId &&
                            b.WarehouseId == dto.WarehouseId &&
                            b.AvailableQuantity > 0 &&
                            b.ExpiryDate > DateTime.UtcNow) // prevent issuing expired stock
                .OrderBy(b => b.ExpiryDate)
                .ThenBy(b => b.CreatedAt)
                .ToListAsync();

            if (!batches.Any())
                throw new InvalidOperationException("No valid batches available.");

            foreach (var batch in batches)
            {
                if (remainingQty <= 0)
                    break;

                var deduction = Math.Min(batch.AvailableQuantity, remainingQty);

                batch.AvailableQuantity -= deduction;
                remainingQty -= deduction;
            }

            if (remainingQty > 0)
                throw new InvalidOperationException("Stock inconsistency detected.");

            // 🔹 Create Issue Record
            var issue = new StockIssueEntity
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.WarehouseId,
                QuantityIssued = dto.QuantityIssued,
                IssueReason = dto.IssueReason,
                CreatedAt = DateTime.UtcNow
            };

            await _context.StockIssues.AddAsync(issue);

            // 🔹 Ledger Entry
            await _context.StockLedgers.AddAsync(new StockLedger
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.WarehouseId,
                Quantity = dto.QuantityIssued,
                MovementType = "OUT",
                ReferenceNumber = $"ISS-{DateTime.UtcNow.Ticks}",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return issue.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}