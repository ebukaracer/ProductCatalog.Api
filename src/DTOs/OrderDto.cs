using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.DTOs;

public record OrderDto(
    int Id,
    DateTime CreatedAt,
    decimal TotalAmount,
    string Status,
    IEnumerable<OrderItemDto> Items
);

public record OrderItemDto(
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

public record CreateOrderRequest(
    [Required] IList<CreateOrderItemDto> Items
);

public record CreateOrderItemDto(
    [Required, Range(1, int.MaxValue)] int ProductId,
    [Required, Range(1, int.MaxValue)] int Quantity
);

public record PlacedOrderDto(
    int Id,
    string Status
);