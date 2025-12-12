using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Api.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    [Precision(18, 2)] public decimal Price { get; set; }
    public int StockQty { get; set; }

    /// <summary>
    /// Rather than have the database manage the concurrency token automatically, you can manage it in application code.
    /// Read more: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; }
}