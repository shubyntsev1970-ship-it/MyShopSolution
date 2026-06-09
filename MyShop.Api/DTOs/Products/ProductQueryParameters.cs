namespace MyShop.Api.DTOs.Products;

public class ProductQueryParameters
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public int? MinStock { get; set; }
    public int? MaxStock { get; set; }

    // Например: name, price, stock, createdAtUtc
    public string? SortBy { get; set; } = "id";

    // asc или desc
    public string? SortDirection { get; set; } = "asc";
}
