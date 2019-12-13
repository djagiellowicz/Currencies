using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using WalutyBusinessLogic.Models.Enums;
using WalutyBusinessLogic.Models.Generic;
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
        public async Task<IActionResult> Delete(string id, Page page)
        {
            // Can be changed from bool to IdentityResult

            bool result = await _userServices.Delete(id);
            ViewData["IsRemoved"] = result;

            return View("Index", await _userServices.GetUsersPage(page.PageNumber, page.PageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id, Page page)
        {
            UserDTO userDTO = await _userServices.GetUser(id);
            UserModel userModel = _mapper.Map<UserDTO, UserModel>(userDTO);

            ViewData["AllRoles"] = _roleManager.Roles.Select(x => x).ToList();

            return View(new PageModel<UserModel>(userModel, page));
        }

        [HttpPost]
        public async Task<IActionResult> Update(PageModel<UserModel> model)
        {
            var isUpdated = await _userServices.Update(model.ViewModel);

            ViewData["IsUpdated"] = isUpdated;

            return View("Index", await _userServices.GetUsersPage(model.Page.PageNumber, model.Page.PageSize));
        }
    }
}