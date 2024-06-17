using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(IdentityUser user, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

        bool ValidateToken(string token);
    }
}
