using System.ComponentModel.DataAnnotations;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Extensions;

/// <summary>
/// Mapping extensions for Product related models and DTOs.
/// </summary>
public static class ProductExtensions
{
    public static ProductDto AsProductDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQty,
            product.Version.ToString());
    }

    public static Product AsProduct(this CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQty = dto.StockQty
        };
    }

    /// <summary>
    /// Update product in such a way that only non-null fields(if any) in the DTO are applied to the existing product(from DB).
    /// </summary>
    /// <param name="dto">Updated product</param>
    /// <param name="product">Existing product</param>
    public static void ApplyPartialUpdate(UpdateProductDto dto, Product product)
    {
        if (dto.Name != null)
        {
            if (dto.Name.Length is < 3 or > 50)
                throw new ValidationException("Name must be 3–50 characters.");

            product.Name = dto.Name;
        }

        if (dto.Description != null)
            product.Description = dto.Description;

        if (dto.Price.HasValue)
        {
            if (dto.Price <= 0)
                throw new ValidationException("Price must be greater than 0.");

            product.Price = dto.Price.Value;
        }

        if (dto.StockQty.HasValue)
        {
            if (dto.StockQty < 0)
                throw new ValidationException("StockQty must be a positive number.");

            product.StockQty = dto.StockQty.Value;
        }
    }
}