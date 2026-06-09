namespace MyShop.Api.DTOs.Products;

// DTO, который API возвращает наружу в React.
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}
