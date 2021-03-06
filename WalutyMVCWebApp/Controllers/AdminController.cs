﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.DatabaseLoading.Updater;
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
        private readonly ICurrencyFilesUpdater _currencyFilesUpdater;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int _defaultPageSize = 5;
        private readonly int _defaultPageNumber = 1;
        private readonly WalutyDBContext _context;
        

        public AdminController(IUserServices userServices, IMapper mapper, RoleManager<IdentityRole> roleManager, 
                               ICurrencyFilesUpdater currencyFilesUpdater, WalutyDBContext context)
        {
            _userServices = userServices;
            _mapper = mapper;
            _roleManager = roleManager;
            _currencyFilesUpdater = currencyFilesUpdater;
            _context = context;
        }

        public async Task<IActionResult> Index(Page page)
        {
            page = GetPageOrDefaultValues(page);

            return View(await _userServices.GetUsersPage(page.PageNumber, page.PageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id, Page page)
        {
            page = GetPageOrDefaultValues(page);

            bool result = await _userServices.Delete(id);
            ViewData["IsRemoved"] = result;

            return View("Index", await _userServices.GetUsersPage(page.PageNumber, page.PageSize));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id, Page page)
        {
            UserDTO userDTO = await _userServices.GetUser(id);
            UserModel userModel = _mapper.Map<UserDTO, UserModel>(userDTO);

            page = GetPageOrDefaultValues(page);

            ViewData["AllRoles"] = _roleManager.Roles.Select(x => x).ToList();

            return View(new PageModel<UserModel>(userModel, page));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDatabase(Page page)
        {
            IList<string> comments = new List<string>();

            bool result = _currencyFilesUpdater.Process(_context);

            if (result)
            {
                comments.Add("Sucessfully updated database");
            }
            else
            {
                comments.Add("Couldn't updated database, see logs for details.");
            }

            ViewData["Comments"] = comments;

            return View("Index", await _userServices.GetUsersPage(page.PageNumber, page.PageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Update(PageModel<UserModel> model)
        {
            UpdateUserResult updateResult = await _userServices.Update(model.ViewModel);
            IList<string> comments = new List<string>();

            model.Page = GetPageOrDefaultValues(model.Page);

            if (updateResult.IsPasswordUpdated)
            {
                comments.Add("Succeeded to update password");
            }
            else
            {
                comments.Add("Password was not updated");
            }

            if (updateResult.AreRolesUpdated)
            {
                comments.Add("Succeeded to update roles");
            }
            else
            {
                comments.Add("Roles were not updated");
            }

            ViewData["Comments"] = comments;

            return View("Index", await _userServices.GetUsersPage(model.Page.PageNumber, model.Page.PageSize));
        }

        private Page GetPageOrDefaultValues(Page page)
        {
            if (page.PageNumber <= 0)
            {
                page.PageNumber = _defaultPageNumber;
            }
            if (page.PageSize <= 0)
            {
                page.PageSize = _defaultPageSize;
            }
            return page;
        }
    }
}