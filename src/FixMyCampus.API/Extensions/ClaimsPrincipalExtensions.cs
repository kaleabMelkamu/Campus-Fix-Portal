using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FixMyCampus.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var idClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (Guid.TryParse(idClaim, out var id))
        {
            return id;
        }

        // Fallback default reporter account for rapid testing
        return Guid.Parse("22222222-2222-2222-2222-222222222222");
    }
}
