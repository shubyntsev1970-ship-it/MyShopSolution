using MyShop.Api.Entities;

namespace MyShop.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<AppUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task AddAsync(AppUser user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
