namespace FixMyCampus.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; set; } = "FixMyCampusSuperSecretKey2026HackathonMustBeLongEnough!";
    public string Issuer { get; set; } = "FixMyCampusAPI";
    public string Audience { get; set; } = "FixMyCampusApp";
    public int ExpiryMinutes { get; set; } = 1440;
}
