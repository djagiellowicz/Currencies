﻿using Microsoft.AspNetCore.Identity;
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
        private readonly string _testUserName = "Mark";
        private readonly string _testUserId = "1";
        private readonly string _firstTestCurrencyName = "USD";
        private readonly int _firstTestCurrencyId = 1;
        private readonly string _secondTestCurrencyName = "AUD";
        private readonly int _secondTestCurrencyId = 2;


        private IUserCurrencyRepository CreateUserCurrencyRepository()
        {
            Mock<IUserCurrencyRepository> userCurrencyRepositoryMock = new Mock<IUserCurrencyRepository>();
            List<Currency> currenciesToReturn;
            User testUser = new User()
            {
                UserName = _testUserName,
                Id = _testUserId,
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
                Id = _firstTestCurrencyId,
                Name = _firstTestCurrencyName,
                FavoritedByUsers = new List<UserCurrency> { testUserCurrency }
            };

            testUserCurrency.Currency = testCurrency;
            currenciesToReturn = new List<Currency>() { testCurrency };

            userCurrencyRepositoryMock.Setup(x => x.GetUserFavoriteCurrencies(testUser.Id)).ReturnsAsync(currenciesToReturn);
            userCurrencyRepositoryMock.Setup(x => x.GetUserCurrency(testUser.Id, testCurrency.Id)).ReturnsAsync(testUserCurrency);

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
                UserName = _testUserName,
                Id = _testUserId,
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
                Id = _firstTestCurrencyId,
                Name = _firstTestCurrencyName,
                FavoritedByUsers = new List<UserCurrency> { testUserCurrency }
            };

            testUserCurrency.Currency = testCurrency;
            testUser.UserFavoriteCurrencies.Add(testUserCurrency);

            var mock = users.AsQueryable().BuildMock();

            users.Add(testUser);

            userManagerMock.Setup(x => x.Users).Returns(mock.Object);

            return userManagerMock.Object;
        }

        private ICurrencyRepository CreateCurrencyRepositoryMock()
        {
            Mock<ICurrencyRepository> currencyRepositoryMock = new Mock<ICurrencyRepository>();
            Currency testCurrency = new Currency()
            {
                Id = _secondTestCurrencyId,
                Name = _secondTestCurrencyName,
                FavoritedByUsers = new List<UserCurrency> { }
            };
            List<Currency> currencies = new List<Currency> { testCurrency };

            currencyRepositoryMock.Setup(x => x.GetCurrency(testCurrency.Id)).ReturnsAsync(testCurrency);

            return currencyRepositoryMock.Object;
        }

        private FavoritesService CreateFavoritesService()
        {
            UserManager<User> userManagerMock = CreateUserManagerMock();
            IUserCurrencyRepository userCurrencyRepository = CreateUserCurrencyRepository();
            ICurrencyRepository currencyRepository = CreateCurrencyRepositoryMock();
            FavoritesService favoritesService = new FavoritesService(userManagerMock, userCurrencyRepository, currencyRepository);

            return favoritesService;
        }

        [Fact]
        public async void FavortiesServiceTests_GetLoggedUserFavCurrencies_Should_Return_1_USD()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, _testUserName) });
            Currency expectedCurrency = new Currency()
            {
                Id = _firstTestCurrencyId,
                Name = _firstTestCurrencyName
            };
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            bool result = false;

            //Act
            var loggedUserFavService = await favoritesService.GetLoggedUserFavCurrencies(user);

            if (loggedUserFavService.Count == 1)
            {
                if (loggedUserFavService[0].Id == expectedCurrency.Id
                    && loggedUserFavService[0].Name == expectedCurrency.Name)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void FavortiesServiceTests_AddFavCurrency_Currency_Is_Added()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, _testUserName) });
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            int currencyId = _secondTestCurrencyId;
            bool result = false;

            //Act
            result = await favoritesService.AddFavCurrency(currencyId, user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void FavortiesServiceTests_AddFavCurrency_Currency_Should_Not_Be_Added()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, _testUserName) });
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            int currencyId = _firstTestCurrencyId;
            bool result = true;

            //Act
            result = await favoritesService.AddFavCurrency(currencyId, user);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async void FavortiesServiceTests_AddFavCurrency_Currency_Is_Deleted()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, _testUserName) });
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            int currencyId = _firstTestCurrencyId;
            bool result = false;

            //Act
            result = await favoritesService.DeleteFavCurrency(currencyId, user);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void FavortiesServiceTests_AddFavCurrency_Currency_Should_Not_Be_Deleted()
        {
            //Arrange
            FavoritesService favoritesService = CreateFavoritesService();
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, _testUserName) });
            ClaimsPrincipal user = new ClaimsPrincipal(claims);
            int currencyId = _secondTestCurrencyId;
            bool result = true;

            //Act
            result = await favoritesService.DeleteFavCurrency(currencyId, user);

            //Assert
            Assert.False(result);
        }
    }
}
