using warehouse_management_system.Database;
using warehouse_management_system.Modules.InventoryMovement.Entities;
using warehouse_management_system.Modules.WarehouseTransfer.DTOs;
using warehouse_management_system.Modules.WarehouseTransfer.Entities;
using warehouse_management_system.Modules.WarehouseTransfer.Interfaces;
using warehouse_management_system.Modules.StockBalance.Interfaces;

namespace warehouse_management_system.Modules.WarehouseTransfer.Services;

public class WarehouseTransferService : IWarehouseTransferService
{
    private readonly ApplicationDbContext _context;
    private readonly IStockBalanceService _stockBalanceService;

    public WarehouseTransferService(
        ApplicationDbContext context,
        IStockBalanceService stockBalanceService)
    {
        _context = context;
        _stockBalanceService = stockBalanceService;
    }

    public async Task<Guid> TransferAsync(CreateTransferDto dto)
    {
        // ✅ Validate sufficient stock in source warehouse
        var balance = await _stockBalanceService
            .GetItemStockAsync(dto.ItemId, dto.SourceWarehouseId);

        if (balance.CurrentStock < dto.Quantity)
            throw new Exception("Insufficient stock for transfer.");

        var transfer = new WarehouseTransferEntity
        {
            ItemId = dto.ItemId,
            SourceWarehouseId = dto.SourceWarehouseId,
            DestinationWarehouseId = dto.DestinationWarehouseId,
            Quantity = dto.Quantity
        };

        await _context.WarehouseTransfers.AddAsync(transfer);

        // 🔥 OUT FROM SOURCE
        await _context.StockLedgers.AddAsync(
            new StockLedger
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.SourceWarehouseId,
                Quantity = dto.Quantity,
                MovementType = "TRANSFER_OUT",
                ReferenceNumber = $"TRF-{DateTime.UtcNow.Ticks}"
            });

        // 🔥 IN TO DESTINATION
        await _context.StockLedgers.AddAsync(
            new StockLedger
            {
                ItemId = dto.ItemId,
                WarehouseId = dto.DestinationWarehouseId,
                Quantity = dto.Quantity,
                MovementType = "TRANSFER_IN",
                ReferenceNumber = $"TRF-{DateTime.UtcNow.Ticks}"
            });

        await _context.SaveChangesAsync();

        return transfer.Id;
    }
}