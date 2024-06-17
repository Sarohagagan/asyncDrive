using asyncDrive.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

namespace asyncDrive.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IApiService _apiService;
        public UserController(IApiService apiService)
        {
            _apiService= apiService;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

    }
}
