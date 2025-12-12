using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.DTOs;
using ProductCatalog.Api.Middlewares;
using ProductCatalog.Api.Repositories;

namespace ProductCatalog.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class ProductsController(IProductRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetAll() =>
        await repository.GetAllAsync();

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto product)
    {
        await repository.AddAsync(product);
        return Ok(product);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await repository.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto updatedProduct)
    {
        try
        {
            await repository.UpdateAsync(id, updatedProduct);
            return NoContent();
        }
        catch (ConcurrencyException)
        {
            return Conflict("Product was updated by someone else.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }
}