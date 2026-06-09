using Microsoft.EntityFrameworkCore;
using MyShop.Api.Data;
using MyShop.Api.DTOs.Products;
using MyShop.Api.Entities;
using MyShop.Api.Repositories.Interfaces;

namespace MyShop.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryParameters query, CancellationToken cancellationToken)
    {
        // IQueryable — это еще НЕ запрос в базу.
        // Мы только собираем запрос по кусочкам.
        IQueryable<Product> productsQuery = _db.Products.AsNoTracking();

        // Поиск по имени или описанию.
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            string search = query.Search.Trim().ToLower();

            productsQuery = productsQuery.Where(product =>
                product.Name.ToLower().Contains(search) ||
                (product.Description != null && product.Description.ToLower().Contains(search)));
        }

        // Фильтр по минимальной цене.
        if (query.MinPrice.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.Price >= query.MinPrice.Value);
        }

        // Фильтр по максимальной цене.
        if (query.MaxPrice.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.Price <= query.MaxPrice.Value);
        }

        // Фильтр по остатку.
        if (query.MinStock.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.Stock >= query.MinStock.Value);
        }

        if (query.MaxStock.HasValue)
        {
            productsQuery = productsQuery.Where(product => product.Stock <= query.MaxStock.Value);
        }

        // Общее количество ДО пагинации.
        int totalCount = await productsQuery.CountAsync(cancellationToken);

        // Сортировка.
        productsQuery = ApplySorting(productsQuery, query.SortBy, query.SortDirection);

        // Пагинация.
        int page = Math.Max(query.Page, 1);
        int pageSize = Math.Clamp(query.PageSize, 1, 100);

        IReadOnlyList<Product> items = await productsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _db.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _db.Products.AddAsync(product, cancellationToken);
        return product;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    public void Delete(Product product)
    {
        _db.Products.Remove(product);
    }
    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy, string? sortDirection)
    {
        bool desc = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return sortBy?.ToLower() switch
        {
            "name" => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
            "price" => desc ? query.OrderByDescending(x => x.Price) : query.OrderBy(x => x.Price),
            "stock" => desc ? query.OrderByDescending(x => x.Stock) : query.OrderBy(x => x.Stock),
            "createdatutc" => desc ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc),
            _ => desc ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id)
        };
    }
}