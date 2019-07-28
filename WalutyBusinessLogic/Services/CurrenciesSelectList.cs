using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{

    public class CurrenciesSelectList : ICurrenciesSelectList
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyRepository _repository;

        public CurrenciesSelectList(UserManager<User> userManager, ICurrencyRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<IEnumerable<SelectListItem>> GetCurrencyCodes(string identityName)
        {
            List<UserCurrency> userFavoriteCurrencies = new List<UserCurrency>();
            List<CurrencyInfo> allCurrencyInfo = await SelectAllCurrencyInfo();

            if (identityName != null)
            {
                var user = await _userManager.Users
                    .Include(u => u.UserFavoriteCurrencies)
                    .ThenInclude(x => x.Currency)
                    .SingleAsync(u => u.UserName == identityName);

                userFavoriteCurrencies = user.UserFavoriteCurrencies;
            }

            return CreateListItems(allCurrencyInfo, userFavoriteCurrencies);
        }

        private async Task<List<CurrencyInfo>> SelectAllCurrencyInfo()
        {
            var listItems = (await _repository.GetAllCurrencyInfo())
                .OrderBy(x => x.Code)
                .ToList();

            return listItems;
        }

        private IEnumerable<SelectListItem> CreateListItems(List<CurrencyInfo> currencyInfo,
            List<UserCurrency> userFavCurrencies)
        {
            List<SelectListItem> sortedToReturn = new List<SelectListItem>();
            bool isFavorite = false;

            foreach (var favCurrency in userFavCurrencies)
            { 
                sortedToReturn.Add(new SelectListItem(favCurrency.Currency.Name, favCurrency.Currency.Name));
            }

            foreach (var currency in currencyInfo)
            {
                isFavorite = false;

                foreach (var favCurrency in userFavCurrencies)
                {
                    if (currency.Code == favCurrency.Currency.Name)
                    {
                        isFavorite = true;
                        break;
                    }
                }
                if (!isFavorite)
                {
                    sortedToReturn.Add(new SelectListItem(currency.Code, currency.Code));
                }
            }

            return sortedToReturn;
        }
    }
}
