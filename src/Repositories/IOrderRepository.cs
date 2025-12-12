using ProductCatalog.Api.DTOs;

namespace ProductCatalog.Api.Repositories;

public interface IOrderRepository
{
    Task<PlacedOrderDto> PlaceOrderAsync(CreateOrderRequest request);

    Task<IEnumerable<OrderDto>> GetPlacedOrdersAsync();

    Task<OrderDto?> GetPlacedOrderAsync(int orderId);
}