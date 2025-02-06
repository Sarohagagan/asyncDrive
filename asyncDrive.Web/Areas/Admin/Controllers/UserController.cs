using asyncDrive.Web.Service.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.DTO;
using Utility;

namespace asyncDrive.Web.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IApiService _apiService;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IApiService apiService, RoleManager<IdentityRole> roleManager)
        {
            _apiService = apiService;
            _roleManager = roleManager;
        }
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [Area("Admin")]
        public IActionResult Register()
        {
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_SiteAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_SuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }
            //start custom code to bind role list
            AddUserRequestDto registerRequestDto = new AddUserRequestDto();

            var RoleList = _roleManager.Roles;
            if (RoleList.Any())
            {
                registerRequestDto.RoleList = new List<Role>();
                foreach (var role in RoleList)
                {
                    var item = new Role();
                    item.Name = role.Name;
                    item.Id = role.Id;
                    registerRequestDto.RoleList.Add(item);
                }
                
            }
            //end custom code to bind role list
            return View(registerRequestDto);
        }
    }
}
