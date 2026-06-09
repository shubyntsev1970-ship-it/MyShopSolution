using Microsoft.EntityFrameworkCore;
using MyShop.Api.Data;
using MyShop.Api.Entities;
using MyShop.Api.Repositories.Interfaces;

namespace MyShop.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return _db.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<AppUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        // Так как BCrypt hash нельзя нормально искать SQL-запросом,
        // для большого production лучше хранить отдельный SHA256 hash refresh token.
        // Для учебного примера читаем пользователей с токенами и проверяем BCrypt.Verify.
        var users = await _db.Users
            .Include(x => x.RefreshTokens)
            .ToListAsync(cancellationToken);

        return users.FirstOrDefault(user =>
            user.RefreshTokens.Any(token =>
                token.IsActive && BCrypt.Net.BCrypt.Verify(refreshToken, token.TokenHash)));
    }
    public async Task AddAsync(AppUser user, CancellationToken cancellationToken)
    {
        await _db.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }
}
