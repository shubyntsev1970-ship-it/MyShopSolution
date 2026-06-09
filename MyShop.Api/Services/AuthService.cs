using MyShop.Api.DTOs.Auth;
using MyShop.Api.Entities;
using MyShop.Api.Repositories.Interfaces;
using MyShop.Api.Services.Interfaces;

namespace MyShop.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenService _jwt;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository users, IJwtTokenService jwt, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _users = users;
        _jwt = jwt;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken)
    {
        AppUser? existingUser = await _users.GetByEmailAsync(dto.Email, cancellationToken);

        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new AppUser
        {
            Email = dto.Email.Trim().ToLower(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "User",
            CreatedAtUtc = DateTime.UtcNow
        };

        string refreshToken = _jwt.CreateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _jwt.HashRefreshToken(refreshToken),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDays"]!))
        });

        await _users.AddAsync(user, cancellationToken);
        await _users.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User registered. Email={Email}", user.Email);

        return CreateAuthResponse(user, refreshToken);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.GetByEmailAsync(dto.Email.Trim().ToLower(), cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        string refreshToken = _jwt.CreateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _jwt.HashRefreshToken(refreshToken),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDays"]!))
        });

        await _users.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User logged in. Email={Email}", user.Email);

        return CreateAuthResponse(user, refreshToken);
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.GetByRefreshTokenAsync(dto.RefreshToken, cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        RefreshToken? oldToken = user.RefreshTokens.FirstOrDefault(token =>
            token.IsActive && BCrypt.Net.BCrypt.Verify(dto.RefreshToken, token.TokenHash));

        if (oldToken == null)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        // Refresh token rotation:
        // старый refresh token отзываем, новый создаем.
        oldToken.RevokedAtUtc = DateTime.UtcNow;

        string newRefreshToken = _jwt.CreateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _jwt.HashRefreshToken(newRefreshToken),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDays"]!))
        });

        await _users.SaveChangesAsync(cancellationToken);

        return CreateAuthResponse(user, newRefreshToken);
    }

    public async Task LogoutAsync(RefreshRequestDto dto, CancellationToken cancellationToken)
    {
        AppUser? user = await _users.GetByRefreshTokenAsync(dto.RefreshToken, cancellationToken);

        if (user == null)
            return;

        RefreshToken? token = user.RefreshTokens.FirstOrDefault(token =>
            token.IsActive && BCrypt.Net.BCrypt.Verify(dto.RefreshToken, token.TokenHash));

        if (token != null)
        {
            token.RevokedAtUtc = DateTime.UtcNow;
            await _users.SaveChangesAsync(cancellationToken);
        }
    }

    private AuthResponseDto CreateAuthResponse(AppUser user, string refreshToken)
    {
        return new AuthResponseDto
        {
            AccessToken = _jwt.CreateAccessToken(user),
            RefreshToken = refreshToken,
            Email = user.Email,
            Role = user.Role
        };
    }
}