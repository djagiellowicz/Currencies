using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
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
        private UserServices CreateUserServices()
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
            Mock<RoleManager<IdentityRole>> roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null,
                null,
                null,
                null);

            //IRoleStore<TRole> store, IEnumerable< IRoleValidator < TRole >> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger

            var dbOptions = new DbContextOptionsBuilder<WalutyDBContext>()
                            .UseInMemoryDatabase(databaseName: "ToDoDb")
                            .Options;
            WalutyDBContext context = new WalutyDBContext(dbOptions);

            User userToReturn = new User() { Id = "1234", Email = "John@john.com" };


            //Give proper user
            userManagerMock.Setup(x => x.FindByIdAsync("1234")).ReturnsAsync(userToReturn);
            userManagerMock.Setup(x => x.GetRolesAsync(userToReturn)).ReturnsAsync(new List<string>());

            UserServices userServices = new UserServices(userManagerMock.Object,
                context,
                iMapper,
                iPasswordValidatorMock.Object,
                roleManagerMock.Object);

            return userServices;

        }

        [Fact]
        public async void CreateUserServices_Correct_User_Is_Returned()
        {
            //Arrange
            UserServices userServices = CreateUserServices();
            UserDTO expectedUserDTO = new UserDTO { Email = "John@john.com", Id = "1234" };
            UserDTO resultDTO;
            bool result = false;

            //Act
            resultDTO = await userServices.GetUser("1234");

            if(resultDTO.Id == expectedUserDTO.Id && resultDTO.Email == expectedUserDTO.Email)
            {
                result = true;
            }

            //Assert
            Assert.True(result);
        }

    }
}
