using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        private readonly IUserCurrencyRepository _userCurrencyRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public FavoritesService(UserManager<User> userManager, IUserCurrencyRepository userCurrencyRepository, ICurrencyRepository currencyRepository)
        {
            _userManager = userManager;
            _userCurrencyRepository = userCurrencyRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<List<Currency>> GetLoggedUserFavCurrencies(ClaimsPrincipal user)
        {
            var loggedInUser = await _userManager.Users
                    .Include(u => u.UserFavoriteCurrencies)
                    .SingleAsync(u => u.UserName == user.Identity.Name);

            List<Currency> currencies = await _userCurrencyRepository.GetUserFavoriteCurrencies(loggedInUser.Id);

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
                var favoriteCurrency = await _currencyRepository.GetCurrency(currencyId);

                if (favoriteCurrency != null)
                {
                    try
                    {
                        _userCurrencyRepository.AddUserFavoriteCurrency(new UserCurrency()
                        {
                            Currency = favoriteCurrency,
                            User = loggedInUser,
                            UserId = loggedInUser.Id,
                            CurrencyId = currencyId
                        });
                        result = true;
                    } catch (ArgumentException e)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public async Task<bool> DeleteFavCurrency(int currencyId, ClaimsPrincipal user)
        {
            bool result = false;

            var loggedInUser = await _userManager.Users
                .Include(u => u.UserFavoriteCurrencies)
                .SingleAsync(u => u.UserName == user.Identity.Name);

            if (loggedInUser != null)
            {
                UserCurrency userCurrency = await _userCurrencyRepository.GetUserCurrency(loggedInUser.Id, currencyId);

                if (userCurrency != null)
                {
                    try
                    {
                        _userCurrencyRepository.DeleteUserFavoriteCurrency(userCurrency);
                        result = true;
                    }
                    catch (ArgumentException e)
                    {
                        result = false;
                    }  
                }
            }

            return result;
        }

    }
}
