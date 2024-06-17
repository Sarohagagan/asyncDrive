namespace asyncDrive.Web.Service.IService
{
    public interface IAuthService
    {
        Task<string> GetAccessTokenAsync(string username, string password);
        Task<string> RefreshTokenAsync();
        Task<bool> ValidateTokenAsync();
    }
}
