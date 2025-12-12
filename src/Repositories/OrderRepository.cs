using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Extensions;
using ProductCatalog.Api.Middlewares;

namespace ProductCatalog.Api.Repositories;

public class OrderRepository(OrderDbContext db) : IOrderRepository
{
    public async Task<PlacedOrderDto> PlaceOrderAsync(CreateOrderRequest request)
    {
        await using var transactionAsync = await db.Database.BeginTransactionAsync();

        // Prevent duplicate product IDs
        var duplicateIds = request.Items
            .GroupBy(x => x.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Count != 0)
            throw new BusinessException($"Duplicate product(s) in order: '{string.Join(",", duplicateIds)}'");

        var ids = request.Items.Select(i => i.ProductId).ToList();

        // Load all products in one query (faster)
        var products = await db.Products
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var item in request.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                throw new NotFoundException($"Product '{item.ProductId}' not found.");

            if (product.StockQty < item.Quantity)
                throw new BusinessException($"Insufficient stock for product '{product.Name}'.");

            // update stock
            product.StockQty -= item.Quantity;
        }

        var order = request.AsOrder();

        await db.Orders.AddAsync(order);
        await db.SaveChangesAsync();
        await transactionAsync.CommitAsync();

        return order.AsPlacedOrderDto();
    }

    public async Task<IEnumerable<OrderDto>> GetPlacedOrdersAsync()
    {
        return await db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(o => new OrderDto(
                o.Id,
                o.CreatedAt,
                o.Items.Sum(i => i.Quantity * i.Product.Price),
                o.ComputedStatus,
                o.Items.Select(i => i.AsOrderItemDto())
            ))
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<OrderDto?> GetPlacedOrderAsync(int orderId)
    {
        return db.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .Select(o => new OrderDto(
                o.Id,
                o.CreatedAt,
                o.Items.Sum(i => i.Quantity * i.Product.Price),
                o.ComputedStatus,
                o.Items.Select(i => i.AsOrderItemDto())
            ))
            .AsSplitQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
}