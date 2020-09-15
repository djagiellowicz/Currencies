using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace Waluty.Tests
{
    public class FavortiesServiceTests
    {
        private WalutyDBContext CreateInMemoryDBContext()
        {
            var dbOptions = new DbContextOptionsBuilder<WalutyDBContext>()
                            .UseInMemoryDatabase(databaseName: "ToDoDb")
                            .Options;

            WalutyDBContext context = new WalutyDBContext(dbOptions);

            return context;
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

            return userManagerMock.Object;
        }

        private FavoritesService CreateFavoritesService()
        {
            UserManager<User> userManagerMock = CreateUserManagerMock();
            WalutyDBContext walutyDBContext = CreateInMemoryDBContext();
            FavoritesService favoritesService = new FavoritesService(userManagerMock, walutyDBContext);

            return favoritesService;
        }




    }
}
