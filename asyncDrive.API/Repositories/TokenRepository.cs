using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using asyncDrive.API.Repositories.IRepository;
using Models.Domain;
using Azure.Core;
using System.Data.SqlClient;

namespace asyncDrive.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly string _connectionString;
        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            _connectionString = configuration["ConnectionStrings:asyncDriveAuthConnectionString"];
        }
        public string GenerateAccessToken(IdentityUser user, List<string> roles)
        {
            //Create claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, user.Id)
            //    }),
            //    Expires = DateTime.Now.AddMinutes(15), // Short expiry for demo purposes
            //    SigningCredentials = credentials,
            //    Claims=new IDictionary<string,string>

            //};
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                //expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["AccessTokenExpiry"])),
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
            };
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public bool ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                }, out SecurityToken validatedToken);
                GetPrincipalFromExpiredToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void StoreToken(string userId, string token)
        {
            var encryptedToken = Encrypt(token);
            var expirationDate = DateTime.UtcNow.AddDays(30); // Set expiration

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO UserTokens (UserId, RefreshToken, ExpirationDate) VALUES (@UserId, @RefreshToken, @ExpirationDate)", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RefreshToken", encryptedToken);
                command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                command.ExecuteNonQuery();
            }
        }
        public void UpdateToken(string userId, string newToken)
        {
            var encryptedToken = Encrypt(newToken);
            var expirationDate = DateTime.UtcNow.AddDays(30); // Set a new expiration

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = new SqlCommand("UPDATE UserTokens SET RefreshToken = @RefreshToken, ExpirationDate = @ExpirationDate WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RefreshToken", encryptedToken);
                command.Parameters.AddWithValue("@ExpirationDate", expirationDate);

                command.ExecuteNonQuery();
            }
        }
        public string RetrieveToken(string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT RefreshToken FROM UserTokens WHERE UserId = @UserId AND ExpirationDate > @Now", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Now", DateTime.UtcNow);

                var encryptedToken = command.ExecuteScalar() as byte[];
                return encryptedToken != null ? Decrypt(encryptedToken) : null;
            }
        }
        private byte[] Encrypt(string plainText)
        {
            // Implement your encryption logic here (e.g., AES)
            // Return the encrypted byte array
            return Encoding.UTF8.GetBytes(plainText);
        }
        private string Decrypt(byte[] encryptedText)
        {
            // Implement your decryption logic here
            // Return the decrypted string
            return Encoding.UTF8.GetString(encryptedText);
        }
    }
}
