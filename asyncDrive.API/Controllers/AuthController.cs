using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using asyncDrive.API.Repositories.IRepository;
using Models.DTO;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using Models.Domain;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace asyncDrive.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        //private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>();
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        //[HttpPost]
        //[Route("Register")]
        //public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        //{
        //    var identityUser = new IdentityUser
        //    {
        //        UserName = registerRequestDto.Username,
        //        Email = registerRequestDto.Username
        //    };

        //    var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

        //    if (identityResult.Succeeded)
        //    {
        //        //Add roles to this User
        //        if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
        //        {
        //            identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
        //            if (identityResult.Succeeded)
        //            {
        //                return Ok("User was registered successfully!");
        //            }
        //        }
        //    }
        //    return BadRequest("Something went wrong");
        //}

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] TokenDto.LoginRequest loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if(user != null) {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    //Get roles for User
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {   //Create token
                        var accessToken = tokenRepository.GenerateAccessToken(user, roles.ToList());
                        var refreshToken = tokenRepository.GenerateRefreshToken();
                        var response = new TokenDto.LoginResponse
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken
                        };
                        //refreshTokens[user.Id] = refreshToken;
                        if (tokenRepository.RetrieveToken(user.Id) != null)
                        {
                            tokenRepository.UpdateToken(user.Id, refreshToken);
                        }
                        else
                        {
                            tokenRepository.StoreToken(user.Id, refreshToken);
                        }
                        return Ok(response);
                    }
                }
            }
            return Unauthorized();
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto.RefreshRequest request)
        {
            var principal = tokenRepository.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token");
            }

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();
            var refreshTokens = tokenRepository.RetrieveToken(userId);
            if (refreshTokens!=null || refreshTokens != request.RefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }
            var user = await userManager.FindByIdAsync(userId);
            //Get roles for User
            if(user == null)
                return Unauthorized();
            var roles = await userManager.GetRolesAsync(user);
            var newAccessToken = tokenRepository.GenerateAccessToken(user, roles.ToList());
            var newRefreshToken = tokenRepository.GenerateRefreshToken();

            //refreshTokens[userId] = newRefreshToken;
            tokenRepository.UpdateToken(user.Id, newRefreshToken);
            var response = new TokenDto.RefreshResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
            return Ok(response);
        }
        //[HttpGet("validate")]
        //[Authorize]
        //public IActionResult ValidateToken()
        //{
        //    // If the code reaches this point, it means the token is valid.
        //    return Ok(new { message = "Token is valid." });
        //}

        [HttpPost("validate")]
        public async Task<bool> ValidateToken([FromBody] TokenDto.ValidateRequest request)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    return false;
                }

                return tokenRepository.ValidateToken(request.AccessToken);
            });
        }
    }
}
