using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;

namespace Waluty.Tests
{
    public class UserServicesTests
    {
        private void CreateUserManagerMoq()
        {
            Mock<UserManager<User>> userManagerMock = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object);


            DbContextOptions contextOptions = new DbContextOptions<WalutyDBContext>();
              

        }

    }
}
