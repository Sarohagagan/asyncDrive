using asyncDrive.Web.Areas.Identity.Pages.Account;
using asyncDrive.Web.Service.IService;
using Models.DTO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace asyncDrive.Web.Service
{
    public class ApiService: IApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var client = _clientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("JWT token not found");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var client = _clientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session;
            var token = session.GetString("AccessToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("JWT token not found");
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync(endpoint, data);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        //private static async Task<string> GetTokenAPI(RegisterModel.InputModel Input)
        //{
        //    // Replace with your API endpoint
        //    string apiUrl = "https://localhost:7075/api/Auth/Login";

        //    // Replace with your username and password
        //    var loginData = new LoginRequestDto
        //    {
        //        UserName = Input.Email,
        //        Password = Input.Password
        //    };
        //    string jwtToken = string.Empty;
        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Serialize the login data to JSON
        //            string json = JsonConvert.SerializeObject(loginData);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            // Send the POST request
        //            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        //            response.EnsureSuccessStatusCode();

        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            // Assuming the response is a JSON object containing a token
        //            var responseData = JsonConvert.DeserializeObject<LoginResponseDto>(responseBody);

        //            jwtToken = responseData.JwtToken;
        //        }
        //        catch (HttpRequestException e)
        //        {
        //            Console.WriteLine("\nException Caught!");
        //            Console.WriteLine("Message :{0} ", e.Message);
        //        }
        //    }
        //    return jwtToken;
        //}
        //public static async Task PostUserData(string userId, RegisterModel.InputModel Input)
        //{
        //    // Replace with your API endpoint
        //    string apiUrl = "https://localhost:7075/api/Users";

        //    // Replace with your API token
        //    string apiToken = await GetTokenAPI(Input);

        //    // Example data to be sent in the POST request
        //    var postData = new AddUserRequestDto
        //    {
        //        FirstName = Input.Name,
        //        LastName = Input.Name,
        //        Email = Input.Email,
        //        Password = Input.Password,
        //        PhoneNumber = Input.PhoneNumber,
        //        PostalCode = Input.PostalCode,
        //        Address = Input.StreetAddress,
        //        City = Input.City,
        //        State = Input.State,
        //        CreatedOn = DateTime.UtcNow,
        //        UpdatedOn = DateTime.UtcNow,
        //        LoginUserId = userId,
        //    };

        //    using (HttpClient client = new HttpClient())
        //    {
        //        try
        //        {
        //            // Add the authorization header with the token
        //            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

        //            // Serialize the data to JSON
        //            string json = JsonConvert.SerializeObject(postData);
        //            var content = new StringContent(json, Encoding.UTF8, "application/json");

        //            // Send the POST request
        //            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

        //            response.EnsureSuccessStatusCode();

        //            string responseBody = await response.Content.ReadAsStringAsync();

        //            // Assuming the response is a JSON object
        //            var responseData = JsonConvert.DeserializeObject<dynamic>(responseBody);

        //            Console.WriteLine(responseData);
        //        }
        //        catch (HttpRequestException e)
        //        {
        //            Console.WriteLine("\nException Caught!");
        //            Console.WriteLine("Message :{0} ", e.Message);
        //        }
        //    }
        //}
    }
}
