using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WalutyBusinessLogic.Models.Enums;
using WalutyBusinessLogic.Services;
using WalutyMVCWebApp.AuthorizeAttributes;

namespace WalutyMVCWebApp.Controllers
{
    [AuthorizeEnumRoles(RolesEnum.Administrator)]
    public class AdminController : Controller
    {
        private readonly IUserServices _userServices;

        public AdminController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<IActionResult> Index()
        {    
            return View(await _userServices.GetUsersPage(1, 3));
        }
    }
}