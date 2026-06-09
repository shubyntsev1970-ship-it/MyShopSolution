namespace MyShop.Api.DTOs.Products;

// DTO для полного обновления товара через PUT.
public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
}
