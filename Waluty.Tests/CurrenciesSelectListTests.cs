using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;
using Microsoft.Extensions.Identity.Core;

namespace Waluty.Tests
{
    class CurrenciesSelectListTests
    {
        private CurrenciesSelectList CreateCurrenciesSelectList()
        {
            var currencyRepositoryMock = new Mock<ICurrencyRepository>();
            var userManagerMock = new Mock<UserManager<User>>();

            List<CurrencyInfo> currencyInfoList = new List<CurrencyInfo>();
            currencyInfoList.Add(new CurrencyInfo("Dollar","USD"));
            currencyInfoList.Add(new CurrencyInfo("Australian Dollar", "AUD"));
            currencyInfoList.Add(new CurrencyInfo("Polish Zloty", "PLN"));
            currencyInfoList.Add(new CurrencyInfo("British Pound", "GBP"));

            User user = new User()
            {
                UserName = "TestUser" ,
                UserFavoriteCurrencies =
                {
                    new UserCurrency(){Currency = new Currency("USD")},
                    new UserCurrency(){Currency = new Currency("AUD")},
                }
            };

            IQueryable<User> usersMock = new List<User>() { user }.AsQueryable<User>();

            currencyRepositoryMock.Setup(x => x.GetAllCurrencyInfo()).ReturnsAsync(currencyInfoList);
            userManagerMock.Setup(x => x.Users).Returns(usersMock);

            return new CurrenciesSelectList(userManagerMock.Object, currencyRepositoryMock.Object);
        }



    }
}
