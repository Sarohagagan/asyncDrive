using asyncDrive.Web.Service.IService;
using IdentityModel.Client;
using Models.DTO;
using Newtonsoft.Json;

public class AuthService: IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> GetAccessTokenAsync(string username, string password)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("https://localhost:7075/api/Auth/Login", new { username, password });

        if (response.IsSuccessStatusCode)
        {
            //var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            string responseBody = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<TokenDto.LoginResponse>(responseBody);
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("AccessToken", responseData.AccessToken);
            session.SetString("RefreshToken", responseData.RefreshToken);
            return responseData.AccessToken;
        }

        throw new Exception("Authentication failed");
    }

    public async Task<string> RefreshTokenAsync()
    {
        var client = _clientFactory.CreateClient();
        var session = _httpContextAccessor.HttpContext.Session;
        var request = new TokenDto.RefreshRequest
        {
            AccessToken = session.GetString("AccessToken"),
            RefreshToken = session.GetString("RefreshToken")
        };
        var response = await client.PostAsJsonAsync("https://localhost:7075/api/Auth/refresh", new { request });

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenDto.RefreshResponse>();
            session.SetString("AccessToken", tokenResponse.AccessToken);
            session.SetString("RefreshToken", tokenResponse.RefreshToken);
            return tokenResponse.AccessToken;
        }

        throw new Exception("Token refresh failed");
    }
    public async Task<bool> ValidateTokenAsync()
    {
        var client = _clientFactory.CreateClient();
        var session = _httpContextAccessor.HttpContext.Session;
        var request = new TokenDto.ValidateRequest
        {
            AccessToken = session.GetString("AccessToken")
        };
        var response = await client.PostAsJsonAsync("https://localhost:7075/api/Auth/validate", new { request });

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        return false;
    }
    
}
