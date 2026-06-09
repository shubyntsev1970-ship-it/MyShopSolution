using MyShop.Api.DTOs.Auth;

namespace MyShop.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken);
    Task<AuthResponseDto> RefreshAsync(RefreshRequestDto dto, CancellationToken cancellationToken);
    Task LogoutAsync(RefreshRequestDto dto, CancellationToken cancellationToken);
}
