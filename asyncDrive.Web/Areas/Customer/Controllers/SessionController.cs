using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace asyncDrive.Web.Areas.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionController()
        {
            _httpContextAccessor = new HttpContextAccessor();
        }

        [HttpGet("data")]
        public IActionResult GetSessionData()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var sessionValue = session.GetString("AccessToken");
            if (sessionValue == null)
            {
                return NotFound("Session data not found.");
            }
            var sessionData = new sessionData
            {
                AccessToken = sessionValue,
                RefreshToken = session.GetString("RefreshToken"),
            };
            return Ok(sessionData);
        }

        //[HttpPost("data")]
        //public IActionResult SetSessionData([FromBody] string value)
        //{
        //    HttpContext.Session.SetString("YourSessionKey", value);
        //    return Ok();
        //}
    }
    class sessionData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
