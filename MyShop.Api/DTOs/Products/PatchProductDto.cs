namespace MyShop.Api.DTOs.Products;

// DTO для частичного обновления товара через PATCH.
// Все поля nullable, потому что клиент может прислать только одно поле.
public class PatchProductDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public string? Description { get; set; }
}
