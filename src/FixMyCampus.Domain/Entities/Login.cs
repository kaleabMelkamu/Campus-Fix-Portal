namespace FixMyCampus.Domain.Entities;
using FixMyCampus.Domain.Enums;

public class Login
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
}