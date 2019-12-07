using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using WalutyBusinessLogic.Models.Enums;
using WalutyBusinessLogic.Services;
using WalutyMVCWebApp.AuthorizeAttributes;

namespace WalutyMVCWebApp.Controllers
{
    [AuthorizeEnumRoles(RolesEnum.Administrator)]
    public class AdminController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IUserServices userServices, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _userServices = userServices;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? 5;

            return View(await _userServices.GetUsersPage((int) pageNumber, (int) pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            // Can be changed from bool to IdentityResult
            // Add sending pageNumber and pageSize to RemoveUser when rediricting to Index.

            bool result = await _userServices.Delete(id);
            ViewData["IsRemoved"] = result;

            return View("Index", await _userServices.GetUsersPage(1,5));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            UserDTO userDTO = await _userServices.GetUser(id);
            UserModel userModel = _mapper.Map<UserDTO, UserModel>(userDTO);

            ViewData["AllRoles"] = _roleManager.Roles.Select(x => x).ToList();

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserModel userModel)
        {
            // Can be changed from bool to IdentityResult
            // Add sending pageNumber and pageSize to RemoveUser when rediricting to Index.

            var isUpdated = await _userServices.Update(userModel);

            ViewData["IsUpdated"] = isUpdated;

            return View("Index", await _userServices.GetUsersPage(1, 5));
        }
    }
}