using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.DatabaseLoading
{
    public interface IUserCurrencyRepository
    {
        Task<List<Currency>> GetUserFavoriteCurrencies(string userId);

        Task<UserCurrency> GetUserFavoriteCurrency(string userId, int currencyId);

        void AddUserFavoriteCurrency(UserCurrency userCurrency);

        void DeleteUserFavoriteCurrency(UserCurrency userCurrency);

    }
}
