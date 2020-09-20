using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public class UserCurrencyRepository : IUserCurrencyRepository
    {
        private readonly WalutyDBContext _context;

        public UserCurrencyRepository(WalutyDBContext context)
        {
            _context = context;
        }

        public async Task<List<Currency>> GetUserFavoriteCurrencies(string userId)
        {
            List<Currency> favCurrencies = await _context.UsersCurrencies.Where(u => u.User.Id == userId).Select(x => x.Currency).ToListAsync();

            return favCurrencies;
        }

        public async Task<UserCurrency> GetUserCurrency(string userId, int currencyId)
        {
            UserCurrency userCurrency = await _context.UsersCurrencies.SingleAsync(x => x.User.Id == userId && x.CurrencyId == currencyId);

            return userCurrency;
        }

        public void AddUserFavoriteCurrency(UserCurrency userCurrency)
        {
            _context.UsersCurrencies.Add(userCurrency);
            _context.SaveChanges();
        }

        public void DeleteUserFavoriteCurrency(UserCurrency userCurrency)
        {
            _context.UsersCurrencies.Remove(userCurrency);
            _context.SaveChanges();
        }

    }
}
