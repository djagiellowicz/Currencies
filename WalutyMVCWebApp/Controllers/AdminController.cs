using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;

namespace WalutyMVCWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}