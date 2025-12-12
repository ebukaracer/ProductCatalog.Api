using ProductCatalog.Api.DTOs;

namespace ProductCatalog.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task AddAsync(CreateProductDto dto);
    Task<ProductDto?> GetByIdAsync(int id);
    Task UpdateAsync(int id, UpdateProductDto dto);
    Task DeleteAsync(int id);
}