using Microsoft.AspNetCore.Mvc;
using MyShop.Api.DTOs.Auth;
using MyShop.Api.Services.Interfaces;

namespace MyShop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _auth.RegisterAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _auth.LoginAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(
        [FromBody] RefreshRequestDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _auth.RefreshAsync(dto, cancellationToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshRequestDto dto,
        CancellationToken cancellationToken)
    {
        await _auth.LogoutAsync(dto, cancellationToken);
        return NoContent();
    }
}
