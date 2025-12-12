using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Extensions;
using ProductCatalog.Api.Middlewares;

namespace ProductCatalog.Api.Repositories;

public class ProductRepository(OrderDbContext db) : IProductRepository
{
    public async Task<IEnumerable<ProductDto>> GetAllAsync() =>
        await db.Products
            .Select(p => p.AsProductDto())
            .AsNoTracking()
            .ToListAsync();

    public async Task AddAsync(CreateProductDto dto)
    {
        db.Products.Add(dto.AsProduct());
        await db.SaveChangesAsync();
    }

    public async Task<ProductDto?> GetByIdAsync(int id) =>
        await db.Products
            .Where(p => p.Id == id)
            .Select(p => p.AsProductDto())
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await db.Products.FindAsync(id)
                      ?? throw new NotFoundException("Product not found.");

        if (dto.Version != product.Version.ToString())
            throw new ConcurrencyException("Product updated by someone else.");

        product.Version = Guid.NewGuid();

        ProductExtensions.ApplyPartialUpdate(dto, product);

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await db.Products.Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }
}