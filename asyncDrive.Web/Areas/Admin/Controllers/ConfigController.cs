using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace asyncDrive.Web.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IOptions<ApiSettings> _appSettings;

        public ConfigController(IOptions<ApiSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appSettings.Value);
        }
    }
}
