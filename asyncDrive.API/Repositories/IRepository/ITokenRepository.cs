using Microsoft.AspNetCore.Identity;

namespace asyncDrive.API.Repositories.IRepository
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
