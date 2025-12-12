namespace ProductCatalog.Api.Models;

/// <summary>
/// Represents an order placed by a customer along with its items(products).
/// </summary>
public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Paid;
    public ICollection<OrderItem> Items { get; set; } = [];


    /// <summary>
    /// A way to simulate order shipping after a certain time has passed.
    /// </summary>
    public string ComputedStatus
    {
        get
        {
            var shipAfter = TimeSpan.FromMinutes(1);

            if (Status == OrderStatus.Paid &&
                DateTime.UtcNow - CreatedAt >= shipAfter)
            {
                return nameof(OrderStatus.Shipped);
            }

            return Status.ToString();
        }
    }
}