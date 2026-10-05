using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FixMyCampus.API.Extensions;
using FixMyCampus.Application.DTOs;
using FixMyCampus.Application.Services;

namespace FixMyCampus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var result = await _authService.LoginAsync(request, ct);
        return Ok(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto request, CancellationToken ct)
    {
        var result = await _authService.RegisterAsync(request, ct);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            UserId = User.GetUserId(),
            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
            Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}
