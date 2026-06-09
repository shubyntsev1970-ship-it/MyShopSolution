using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Api.DTOs.Common;
using MyShop.Api.DTOs.Products;
using MyShop.Api.Services.Interfaces;

namespace MyShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;

    public ProductsController(IProductService products)
    {
        _products = products;
    }
    
    // GET /api/products?page=1&pageSize=10&search=phone&sortBy=price&sortDirection=desc
    // Доступен всем авторизованным пользователям.
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<ProductDto>>> GetProducts(
    [FromQuery] ProductQueryParameters query,
    CancellationToken cancellationToken)
    {
        var result = await _products.GetPagedAsync(query, cancellationToken);
        return Ok(result);
    }

    // GET /api/products/5
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken cancellationToken)
    {
        var product = await _products.GetByIdAsync(id, cancellationToken);
        return Ok(product);
    }

    // POST /api/products
    // Только Admin может создавать товары.
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> CreateProduct(
        [FromBody] CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        var createdProduct = await _products.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(GetProductById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    // PUT /api/products/5
    // PUT — полное обновление товара.
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(
        int id,
        [FromBody] UpdateProductDto dto,
        CancellationToken cancellationToken)
    {
        var updatedProduct = await _products.UpdateAsync(id, dto, cancellationToken);
        return Ok(updatedProduct);
    }

    // PATCH /api/products/5
    // PATCH — частичное обновление товара.
    [HttpPatch("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> PatchProduct(
        int id,
        [FromBody] PatchProductDto dto,
        CancellationToken cancellationToken)
    {
        var updatedProduct = await _products.PatchAsync(id, dto, cancellationToken);
        return Ok(updatedProduct);
    }

    // DELETE /api/products/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
    {
        await _products.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
