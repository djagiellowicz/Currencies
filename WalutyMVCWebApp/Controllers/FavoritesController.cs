﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.Enums;
using WalutyBusinessLogic.Services;
using WalutyMVCWebApp.AuthorizeAttributes;

namespace WalutyMVCWebApp.Controllers
{
    [AuthorizeEnumRoles(RolesEnum.Administrator,RolesEnum.User)]
    public class FavoritesController : Controller
    {
        // Remember to create separate controller for these methods, pushed due to deadline

        private readonly UserManager<User> _userManager;
        private readonly WalutyDBContext _context;
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(UserManager<User> userManager, WalutyDBContext context, IFavoritesService favoritesService)
        {
            _userManager = userManager;
            _context = context;
            _favoritesService = favoritesService;
        }
        // GET: Favorites
        public async Task<ActionResult> Index()
        {
            ClaimsPrincipal logged = User;

            List<Currency> currencies = await _favoritesService.GetLoggedUserFavCurrencies(User);

            return View(currencies);
        }

        public async Task<ActionResult> Add(int currencyId)
        {
            var loggedInUser = await _userManager.Users
                .Include(u => u.UserFavoriteCurrencies)
                .SingleAsync(u => u.UserName == User.Identity.Name);

            var favoriteCurrency = _context.Currencies.Find(currencyId);

            _context.UsersCurrencies.Add(new UserCurrency()
            {
                Currency = favoriteCurrency,
                User = loggedInUser,
                UserId = loggedInUser.Id,
                CurrencyId = currencyId
            });
            
            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        // POST: Favorites/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int currencyId)
        {
            var loggedInUser = await _userManager.Users.Include(u => u.UserFavoriteCurrencies)
                .SingleAsync(u => u.UserName == User.Identity.Name);

            var userCurrencies = _context.UsersCurrencies.Single(x => x.User.Id == loggedInUser.Id && x.CurrencyId == currencyId);

            _context.UsersCurrencies.Remove(userCurrencies);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}