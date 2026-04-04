using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace review_microservice;

internal static class JwtUserClaims
{
    /// <summary>
    /// Token z user-microservice ma claim "sub"; NameIdentifier nie zawsze jest mapowany tak samo w JwtBearer.
    /// </summary>
    public static string? GetUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
}
