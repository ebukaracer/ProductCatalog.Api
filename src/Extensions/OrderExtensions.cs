using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Extensions;

/// <summary>
/// Mapping extensions for Order related models and DTOs.
/// </summary>
public static class OrderExtensions
{
    public static Order AsOrder(this CreateOrderRequest request)
    {
        var order = new Order
        {
            Items = request.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        return order;
    }

    public static OrderItemDto AsOrderItemDto(this OrderItem item) =>
        new(item.ProductId, item.Product.Name, item.Quantity, item.Product.Price);

    public static PlacedOrderDto AsPlacedOrderDto(this Order order) =>
        new(order.Id, order.ComputedStatus);
}