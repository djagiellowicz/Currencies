﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(int? pageNumber, int? pageSize)
        {
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? 5;

            return View(await _userServices.GetUsersPage((int) pageNumber, (int) pageSize));
        }

        public async Task<IActionResult> RemoveUser(string id)
        {
            // Add sending pageNumber and pageSize to RemoveUser when rediricting to Index.

            bool result = await _userServices.RemoveUser(id);
            ViewData["IsRemoved"] = result;

            return View("Index", await _userServices.GetUsersPage(1,5));
        }
    }
}