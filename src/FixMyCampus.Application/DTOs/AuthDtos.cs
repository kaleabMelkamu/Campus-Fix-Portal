using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Application.DTOs;

public record LoginRequestDto
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record RegisterRequestDto
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public UserRole Role { get; init; } = UserRole.Reporter;
}

public record AuthResponseDto
{
    public string Token { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
