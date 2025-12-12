using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class OrdersController(IOrderRepository orderRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CreateOrderRequest request)
    {
        var placedOrder = await orderRepository.PlaceOrderAsync(request);
        return CreatedAtAction(nameof(GetPlacedOrder), new { orderId = placedOrder.Id }, new { order = placedOrder });
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<OrderDto>> GetPlacedOrder(int orderId)
    {
        var order = await orderRepository.GetPlacedOrderAsync(orderId);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet]
    public async Task<IEnumerable<OrderDto>> GetPlacedOrders() => await orderRepository.GetPlacedOrdersAsync();
}