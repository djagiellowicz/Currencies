﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(IUserServices userServices, IMapper mapper)
        {
            _userServices = userServices;
            _mapper = mapper;
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
            UserPasswordModel userPasswordModel = _mapper.Map<UserDTO, UserPasswordModel>(userDTO);
            
            return View(userPasswordModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserPasswordModel userPasswordModel)
        {
            // Can be changed from bool to IdentityResult
            // Add sending pageNumber and pageSize to RemoveUser when rediricting to Index.

            var result = await _userServices.Update(userPasswordModel);
            ViewData["IsUpdated"] = result;

            // Should redirect to list of users.

            return RedirectToAction("Index");
        }
    }
}