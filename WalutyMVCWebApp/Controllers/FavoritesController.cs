using System.Collections.Generic;
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
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }
        // GET: Favorites
        public async Task<ActionResult> Index()
        {
            ClaimsPrincipal loggedUser = User;
            List<Currency> currencies = await _favoritesService.GetLoggedUserFavCurrencies(loggedUser);

            return View(currencies);
        }

        public async Task<ActionResult> Add(int currencyId)
        {
            ClaimsPrincipal loggedUser = User;
            bool result = await _favoritesService.AddFavCurrency(currencyId, loggedUser);

            return RedirectToAction("Index","Home");
        }

        // POST: Favorites/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int currencyId)
        {
            ClaimsPrincipal loggedUser = User;
            bool result = await _favoritesService.DeleteFavCurrency(currencyId, loggedUser);

            return RedirectToAction("Index", "Home");
        }
    }
}