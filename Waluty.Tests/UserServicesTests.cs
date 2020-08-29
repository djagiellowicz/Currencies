using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WalutyBusinessLogic.AutoMapper.Profiles;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using WalutyBusinessLogic.Services;
using X.PagedList;
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

            IdentityRole identityRoleAdmin = new IdentityRole("Admin");
            IdentityRole identityRoleUser = new IdentityRole("User");
            List<IdentityRole> roles = new List<IdentityRole>() { identityRoleAdmin, identityRoleUser };
            
            //IRoleStore<TRole> store, IEnumerable< IRoleValidator < TRole >> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger

            var dbOptions = new DbContextOptionsBuilder<WalutyDBContext>()
                            .UseInMemoryDatabase(databaseName: "ToDoDb")
                            .Options;
            WalutyDBContext context = new WalutyDBContext(dbOptions);

            User userToReturn = new User() { Id = "1234", Email = "John@john.com" };
            List<User> usersToReturn = new List<User>() { userToReturn };

            


            //Give proper user
            userManagerMock.Setup(x => x.FindByIdAsync("1234")).ReturnsAsync(userToReturn);
            userManagerMock.Setup(x => x.GetRolesAsync(userToReturn)).ReturnsAsync(new List<string>());
            userManagerMock.Setup(x => x.Users).Returns(usersToReturn.AsQueryable().BuildMock().Object);
            userManagerMock.Setup(x => x.DeleteAsync(userToReturn)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(userToReturn, identityRoleUser.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(userToReturn, identityRoleAdmin.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(userToReturn, identityRoleUser.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(userToReturn, identityRoleAdmin.Name)).ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(x => x.Roles).Returns(roles.AsQueryable().BuildMock().Object);

            UserServices userServices = new UserServices(userManagerMock.Object,
                context,
                iMapper,
                iPasswordValidatorMock.Object,
                roleManagerMock.Object);

            return userServices;

        }

        [Fact]
        public async void UserServices_Get_Correct_Page()
        {
            //Arrange
            UserServices userServices = CreateUserServices();
            UserDTO expectedUserDTO = new UserDTO { Email = "John@john.com", Id = "1234" };
            List<UserDTO> expectedUserDTOs = new List<UserDTO>() { expectedUserDTO };
            IPagedList<UserDTO> expectedDTOList = new StaticPagedList<UserDTO>(expectedUserDTOs,1,1,1);
            IPagedList<UserDTO> resultDTOList;
            bool result = false;

            //Act
             resultDTOList = await userServices.GetUsersPage(1 , 1);

            if(expectedDTOList.PageNumber == resultDTOList.PageNumber &&
               expectedDTOList.PageSize == resultDTOList.PageSize &&
               expectedDTOList.TotalItemCount == resultDTOList.TotalItemCount)
            {
                if(resultDTOList[0].Email == expectedUserDTO.Email &&
                   resultDTOList[0].Id == expectedUserDTO.Id)
                {
                    result = true;
                }
            }

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void UserServices_Correct_User_Is_Returned()
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

        [Fact]
        public async void UserServices_User_Has_Been_Properly_Deleted()
        {
            //Arrange
            UserServices userServices = CreateUserServices();
            string userId = "1234";
            bool result = false;

            //Act
            result = await userServices.Delete(userId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async void UserServices_User_Roles_Have_Been_Properly_Updated()
        {
            //Arrange
            UserServices userServices = CreateUserServices();
            string userId = "1234";
            UserModel userModelToUpdate = new UserModel();
            userModelToUpdate.Id = userId;
            userModelToUpdate.Roles = new List<string>() { "User" };
            userModelToUpdate.NewRoles = new List<string>() { "Admin", "User" };
            UpdateUserResult updateResult = null;

            //Act

            updateResult = await userServices.Update(userModelToUpdate);

            //Assert
            Assert.True(updateResult.AreRolesUpdated);
        }

    }
}
