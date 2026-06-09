namespace MyShop.Api.DTOs.Products;

// DTO для создания товара.
// Тут нет Id, потому что Id создает база данных.
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
}