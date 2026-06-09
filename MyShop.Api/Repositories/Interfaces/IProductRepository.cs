using MyShop.Api.DTOs.Products;
using MyShop.Api.Entities;

namespace MyShop.Api.Repositories.Interfaces;

public interface IProductRepository
{
    Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryParameters query, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void Delete(Product product);
}
