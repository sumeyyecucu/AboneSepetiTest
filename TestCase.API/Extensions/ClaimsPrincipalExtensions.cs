using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace TestCase.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principalExtensions)
    {
        ArgumentNullException.ThrowIfNull(principalExtensions);
        return principalExtensions.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}