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
using MockQueryable.Moq;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            currencyInfoList.Add(new CurrencyInfo("Dollar", "USD"));
            currencyInfoList.Add(new CurrencyInfo("Australian Dollar", "AUD"));
            currencyInfoList.Add(new CurrencyInfo("Polish Zloty", "PLN"));
            currencyInfoList.Add(new CurrencyInfo("British Pound", "GBP"));
            currencyInfoList.Add(new CurrencyInfo("Euro", "EUR"));

            User user = GetTestUser();


            var users = new List<User>() { user };

            var mock = users.AsQueryable().BuildMock();

            currencyRepositoryMock.Setup(x => x.GetAllCurrencyInfo()).ReturnsAsync(currencyInfoList);
            userManagerMock.Setup(x => x.Users).Returns(mock.Object);

            return new CurrenciesSelectList(userManagerMock.Object, currencyRepositoryMock.Object);
        }

        private User GetTestUser()
        {
            User user = new User()
            {
                Id = "Test",
                UserName = "TestUser",

            };
            user.UserFavoriteCurrencies = new List<UserCurrency>()
                {
                    new UserCurrency() { Currency = new Currency("USD"), CurrencyId = 0, User = user, UserId = user.Id },
                    new UserCurrency() { Currency = new Currency("AUD"), CurrencyId = 0, User = user, UserId = user.Id  },
                };

            return user;
        }

        [Fact]
        public async void CurrenciesSelectList_Favorites_Are_At_The_Beggining_Of_The_List()
        {

            //Arrange
            User testUser = GetTestUser();
            List<UserCurrency> usersFavorites = testUser.UserFavoriteCurrencies;
            IEnumerable<SelectListItem> allegedListOfFavorites;
            CurrenciesSelectList currenciesSelectList = CreateCurrenciesSelectList();
            var getCurrencyCodesResult = await currenciesSelectList.GetCurrencyCodes(testUser.UserName);    
            bool testResult = true;



            //Act
            allegedListOfFavorites = getCurrencyCodesResult.Take(testUser.UserFavoriteCurrencies.Count);

            foreach(var currency in allegedListOfFavorites)
            {
                for(int i = 0; i <= usersFavorites.Count; i++)
                {
                    if(currency.Text == usersFavorites[i].Currency.Name)
                    {
                        testResult = true;
                        break;
                    }
                    else
                    {
                        testResult = false;
                    }
                }
            }

            //Assert

            Assert.True(testResult);
        }

    }
}
