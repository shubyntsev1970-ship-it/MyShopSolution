using MyShop.Api.DTOs.Common;
using MyShop.Api.DTOs.Products;

namespace MyShop.Api.Services.Interfaces;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryParameters query, CancellationToken cancellationToken);
    Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken);
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken);
    Task<ProductDto> PatchAsync(int id, PatchProductDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
