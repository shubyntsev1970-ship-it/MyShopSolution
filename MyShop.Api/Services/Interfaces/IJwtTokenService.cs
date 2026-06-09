using MyShop.Api.Entities;

namespace MyShop.Api.Services.Interfaces;

public interface IJwtTokenService
{
    string CreateAccessToken(AppUser user);
    string CreateRefreshToken();
    string HashRefreshToken(string refreshToken);
}
