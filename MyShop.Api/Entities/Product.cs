namespace MyShop.Api.Entities;

// Entity — это C#-класс, который соответствует таблице в базе данных.
// Этот класс соответствует таблице public.products.
public class Product
{
    // id integer primary key
    public int Id { get; set; }

    // name varchar(255) not null
    public string Name { get; set; } = string.Empty;

    // price numeric(18,2) not null
    public decimal Price { get; set; }

    // stock integer not null
    public int Stock { get; set; }

    // description text nullable
    public string? Description { get; set; }

    // created_at_utc timestamp not null default now()
    public DateTime CreatedAtUtc { get; set; }
}
