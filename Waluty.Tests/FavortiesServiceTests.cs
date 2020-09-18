using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;
using Xunit;
using MockQueryable.Moq;

namespace Waluty.Tests
{
    public class FavortiesServiceTests
    {
        private IUserCurrencyRepository CreateUserCurrencyRepository()
        {
            Mock<IUserCurrencyRepository> userCurrencyRepositoryMock = new Mock<IUserCurrencyRepository>();
            

            User testUser = new User()
            {
                UserName = "Mark",
                Id = "1",
                UserFavoriteCurrencies = new List<UserCurrency>()
            };
            UserCurrency testUserCurrency = new UserCurrency()
            {
                CurrencyId = 1,
                Currency = null,
                UserId = testUser.Id,
                User = testUser,
            };
            Currency testCurrency = new Currency()
            {
                Id = 1,
                Name = "USD",
                FavoritedByUsers = new List<UserCurrency> { testUserCurrency }
            };
            testUserCurrency.Currency = testCurrency;

            List<Currency> currenciesToReturn = new List<Currency>() { testCurrency };

            userCurrencyRepositoryMock.Setup(x => x.GetUserFavoriteCurrencies(testUser.Id)).ReturnsAsync(currenciesToReturn);


            return userCurrencyRepositoryMock.Object;
        }


        private UserManager<User> CreateUserManagerMock()
        {
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

            List<User> users = new List<User>();


            User testUser = new User()
            {
                UserName = "Mark",
                Id = "1",
                UserFavoriteCurrencies = new List<UserCurrency>()
            };
            UserCurrency testUserCurrency = new UserCurrency()
            {
                CurrencyId = 1,
                Currency = null,
                UserId = testUser.Id,
                User = testUser,
            };
            Currency testCurrency = new Currency()
            {
                Id = 1,
                Name = "USD",
                FavoritedByUsers = new List<UserCurrency> { testUserCurrency }
            };
            testUserCurrency.Currency = testCurrency;
            testUser.UserFavoriteCurrencies.Add(testUserCurrency);

            var mock = users.AsQueryable().BuildMock();

            users.Add(testUser);

            userManagerMock.Setup(x => x.Users).Returns(mock.Object);

            return userManagerMock.Object;
        }

        private FavoritesService CreateFavoritesService()
        {
            UserManager<User> userManagerMock = CreateUserManagerMock();
            IUserCurrencyRepository userCurrencyRepository = CreateUserCurrencyRepository();
            ICurrencyRepository currencyRepository = new Mock<ICurrencyRepository>().Object;
            FavoritesService favoritesService = new FavoritesService(userManagerMock, userCurrencyRepository, currencyRepository);

            return favoritesService;
        }


        [Fact]
        public async void FavortiesServiceTests_GetLoggedUserFavCurrencies_Should_Return_1_USD()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, "Mark") });
            Currency expectedCurrency = new Currency()
            {
                Id = 1,
                Name = "USD"
            };
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            bool result = false;

            //Act
            var loggedUserFavService = await favoritesService.GetLoggedUserFavCurrencies(user);
            
            if(loggedUserFavService.Count == 1)
            {
                if(loggedUserFavService[0].Id == expectedCurrency.Id 
                    && loggedUserFavService[0].Name == expectedCurrency.Name)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }
    }
}
