using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public class FavoritesService : IFavoritesService
    {
        private readonly UserManager<User> _userManager;
        private readonly WalutyDBContext _context;

        public FavoritesService(UserManager<User> userManager, WalutyDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<List<Currency>> GetLoggedUserFavCurrenciesAsync(ClaimsPrincipal user)
        {
            var loggedInUser = await _userManager.Users
                    .Include(u => u.UserFavoriteCurrencies)
                    .SingleAsync(u => u.UserName == user.Identity.Name);

            List<Currency> currencies = _context.UsersCurrencies.Where(u => u.User.Id == loggedInUser.Id).Select(x => x.Currency).ToList();

            return currencies;
        }
    }
}
