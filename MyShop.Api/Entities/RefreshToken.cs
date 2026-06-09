namespace MyShop.Api.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    // Храним hash refresh token, а не сам токен.
    public string TokenHash { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAtUtc { get; set; }

    public int UserId { get; set; }

    public AppUser User { get; set; } = null!;

    public bool IsActive => RevokedAtUtc == null && ExpiresAtUtc > DateTime.UtcNow;
}
