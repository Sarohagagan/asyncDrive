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
        void StoreToken(string userId, string token);
        void UpdateToken(string userId, string newToken);
        string RetrieveToken(string userId);
    }
}
