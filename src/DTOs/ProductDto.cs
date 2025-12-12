using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Api.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int StockQty,
    string? Version
);

public record CreateProductDto(
    [Required, MinLength(3), MaxLength(50)]
    string Name,
    string? Description,
    [Required, Range(1, int.MaxValue)] decimal Price,
    [Required, Range(1, int.MaxValue)] int StockQty
);

public record UpdateProductDto(
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQty,
    string? Version
);