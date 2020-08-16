using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using WalutyBusinessLogic.AutoMapper.Profiles;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using WalutyBusinessLogic.Services;
using Xunit;

namespace Waluty.Tests
{
    public class UserServicesTests
    {
        private void CreateUserServices()
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

            IMapper iMapper = new MapperConfiguration(c => c.AddProfile<UserProfileMap>()).CreateMapper();

            Mock<IPasswordValidator<User>> iPasswordValidatorMock = new Mock<IPasswordValidator<User>>();
            Mock<RoleManager<IdentityRole>> roleManagerMock = new Mock<RoleManager<IdentityRole>>();

            DbContextOptions contextOptions = new DbContextOptions<WalutyDBContext>();
            var options = new DbContextOptionsBuilder<WalutyDBContext>().UseInMemoryDatabase("test").Options;

            WalutyDBContext context = new WalutyDBContext(options);

            User userToReturn = new User() { Id = "1234", Email = "John@john.com" };


            //Give proper user
            userManagerMock.Setup(x => x.FindByIdAsync("1234")).ReturnsAsync(userToReturn);

            UserServices userServices = new UserServices(userManagerMock.Object,
                context,
                iMapper,
                iPasswordValidatorMock.Object,
                roleManagerMock.Object);

        }

        [Fact]
        public void CreateUserServices_Correct_User_Is_Returned()
        {

        }

    }
}
