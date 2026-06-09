using MyShop.Api.DTOs.Common;
using MyShop.Api.DTOs.Products;
using MyShop.Api.Entities;
using MyShop.Api.Repositories.Interfaces;
using MyShop.Api.Services.Interfaces;

namespace MyShop.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryParameters query, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(query, cancellationToken);

        return new PagedResult<ProductDto>
        {
            Items = items.Select(ToDto).ToList(),
            Page = Math.Max(query.Page, 1),
            PageSize = Math.Clamp(query.PageSize, 1, 100),
            TotalCount = totalCount
        };
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        Product product = await GetProductOrThrowAsync(id, cancellationToken);
        return ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken)
    {
        Product product = new()
        {
            Name = dto.Name,
            Price = dto.Price,
            Stock = dto.Stock,
            Description = dto.Description,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(product, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product created. Id={ProductId}, Name={ProductName}", product.Id, product.Name);

        return ToDto(product);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken)
    {
        Product product = await GetProductOrThrowAsync(id, cancellationToken);

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.Description = dto.Description;

        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product updated. Id={ProductId}", product.Id);

        return ToDto(product);
    }

    public async Task<ProductDto> PatchAsync(int id, PatchProductDto dto, CancellationToken cancellationToken)
    {
        Product product = await GetProductOrThrowAsync(id, cancellationToken);

        if (dto.Name != null)
            product.Name = dto.Name;

        if (dto.Price.HasValue)
            product.Price = dto.Price.Value;

        if (dto.Stock.HasValue)
            product.Stock = dto.Stock.Value;

        if (dto.Description != null)
            product.Description = dto.Description;

        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product patched. Id={ProductId}", product.Id);

        return ToDto(product);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        Product product = await GetProductOrThrowAsync(id, cancellationToken);

        _repository.Delete(product);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Product deleted. Id={ProductId}", product.Id);
    }

    private async Task<Product> GetProductOrThrowAsync(int id, CancellationToken cancellationToken)
    {
        Product? product = await _repository.GetByIdAsync(id, cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} was not found");
        }

        return product;
    }

    private static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Description = product.Description,
            CreatedAtUtc = product.CreatedAtUtc
        };
    }
}