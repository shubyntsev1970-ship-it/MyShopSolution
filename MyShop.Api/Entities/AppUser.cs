namespace MyShop.Api.Entities;

public class AppUser
{
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    // Тут лежит НЕ пароль, а BCrypt hash пароля.
    public string PasswordHash { get; set; } = string.Empty;

    // Простая роль: Admin или User.
    public string Role { get; set; } = "User";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public List<RefreshToken> RefreshTokens { get; set; } = new();
}