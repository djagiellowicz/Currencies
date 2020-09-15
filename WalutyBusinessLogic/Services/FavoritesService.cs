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

        public async Task<List<Currency>> GetLoggedUserFavCurrencies(ClaimsPrincipal user)
        {
            var loggedInUser = await _userManager.Users
                    .Include(u => u.UserFavoriteCurrencies)
                    .SingleAsync(u => u.UserName == user.Identity.Name);

            List<Currency> currencies = _context.UsersCurrencies.Where(u => u.User.Id == loggedInUser.Id).Select(x => x.Currency).ToList();

            return currencies;
        }

        public async Task<bool> AddFavCurrency(int currencyId, ClaimsPrincipal user)
        {
            bool result = false;

            var loggedInUser = await _userManager.Users
                .Include(u => u.UserFavoriteCurrencies)
                .SingleAsync(u => u.UserName == user.Identity.Name);

            if (loggedInUser != null)
            {

                var favoriteCurrency = _context.Currencies.Find(currencyId);

                if (favoriteCurrency != null)
                {

                    _context.UsersCurrencies.Add(new UserCurrency()
                    {
                        Currency = favoriteCurrency,
                        User = loggedInUser,
                        UserId = loggedInUser.Id,
                        CurrencyId = currencyId
                    });

                    _context.SaveChanges();

                    result = true;

                }
            }

            return result;
        }

        public async Task<bool> DeleteFavCurrency(int currencyId, ClaimsPrincipal user)
        {
            bool result = false;

            var loggedInUser = await _userManager.Users.Include(u => u.UserFavoriteCurrencies)
                .SingleAsync(u => u.UserName == user.Identity.Name);

            if (loggedInUser != null)
            {
                var userCurrencies = _context.UsersCurrencies.Single(x => x.User.Id == loggedInUser.Id && x.CurrencyId == currencyId);

                if (userCurrencies != null)
                {
                    _context.UsersCurrencies.Remove(userCurrencies);
                    _context.SaveChanges();

                    result = true;
                }
            }

            return result;
        }

    }
}
