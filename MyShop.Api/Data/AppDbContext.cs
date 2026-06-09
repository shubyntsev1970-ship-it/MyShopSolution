using Microsoft.EntityFrameworkCore;
using MyShop.Api.Entities;

namespace MyShop.Api.Data;

// DbContext — главный класс EF Core для общения с базой данных.
// Через него C# понимает, какие таблицы есть в базе.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Таблица products.
    public DbSet<Product> Products => Set<Product>();

    // Таблица users.
    public DbSet<AppUser> Users => Set<AppUser>();

    // Таблица refresh_tokens.
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("id");

            entity.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasColumnName("price")
                .HasPrecision(18, 2)
                .IsRequired();

            entity.Property(x => x.Stock)
                .HasColumnName("stock")
                .IsRequired();

            entity.Property(x => x.Description)
                .HasColumnName("description");

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("users", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();

            entity.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            entity.Property(x => x.Role)
                .HasColumnName("role")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired();
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.TokenHash)
                .HasColumnName("token_hash")
                .IsRequired();

            entity.Property(x => x.ExpiresAtUtc)
                .HasColumnName("expires_at_utc")
                .IsRequired();

            entity.Property(x => x.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .IsRequired();

            entity.Property(x => x.RevokedAtUtc)
                .HasColumnName("revoked_at_utc");

            entity.Property(x => x.UserId)
                .HasColumnName("user_id");

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId);
        });
    }
}



