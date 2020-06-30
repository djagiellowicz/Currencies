using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class CurrenciesSelectListTests
    {
        private CurrenciesSelectList CreateCurrenciesSelectList()
        {
            var currencyRepositoryMock = new Mock<ICurrencyRepository>();
            var userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);      

            List<CurrencyInfo> currencyInfoList = new List<CurrencyInfo>();
            currencyInfoList.Add(new CurrencyInfo("Dollar","USD"));
            currencyInfoList.Add(new CurrencyInfo("Australian Dollar", "AUD"));
            currencyInfoList.Add(new CurrencyInfo("Polish Zloty", "PLN"));
            currencyInfoList.Add(new CurrencyInfo("British Pound", "GBP"));

            User user = new User()
                {
                    Id = "Test",
                    UserName = "TestUser"   
                };
            user.UserFavoriteCurrencies = new List<UserCurrency>()
                {
                    new UserCurrency() { Currency = new Currency("USD"), CurrencyId = 1, User = user, UserId = user.Id },
                    new UserCurrency() { Currency = new Currency("AUD"), CurrencyId = 2, User = user, UserId = user.Id  },
                };

            IQueryable<User> usersMock = new List<User>() { user }.AsQueryable<User>();

            currencyRepositoryMock.Setup(x => x.GetAllCurrencyInfo()).ReturnsAsync(currencyInfoList);
            userManagerMock.SetupGet(x => x.Users).Returns(usersMock);

            return new CurrenciesSelectList(userManagerMock.Object, currencyRepositoryMock.Object);
        }

        [Fact]
        public async void CurrenciesSelectList_Proper_Favorites_Are_Returned()
        {
            CurrenciesSelectList currenciesSelectList = CreateCurrenciesSelectList();
            var result = await currenciesSelectList.GetCurrencyCodes("TestUser");
        }



    }
}
