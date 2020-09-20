using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using WalutyBusinessLogic.AutoMapper.Profiles;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using WalutyBusinessLogic.Services;
using X.PagedList;
using Xunit;

namespace Waluty.Tests
{
    public class UserServicesTests
    {
        private IdentityRole _userRole = new IdentityRole("User");
        private IdentityRole _adminRole = new IdentityRole("Admin");
        private User _testUser = new User() { Id = "1234", Email = "John@john.com" };

        private UserServices CreateUserServices()
        {
            Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();
            Mock<RoleManager<IdentityRole>> roleManagerMock = CreateRoleManagerMock();
            Mock<IPasswordValidator<User>> iPasswordValidatorMock = new Mock<IPasswordValidator<User>>();
            IMapper iMapper = CreateMapper();

            UserServices userServices = new UserServices(userManagerMock.Object,
                iMapper,
                iPasswordValidatorMock.Object,
                roleManagerMock.Object);

            return userServices;

        }

        private Mock<UserManager<User>> CreateUserManagerMock()
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

            
            List<User> testUsers = new List<User>() { _testUser };

            userManagerMock.Setup(x => x.FindByIdAsync("1234")).ReturnsAsync(_testUser);
            userManagerMock.Setup(x => x.GetRolesAsync(_testUser)).ReturnsAsync(new List<string>());
            userManagerMock.Setup(x => x.Users).Returns(testUsers.AsQueryable().BuildMock().Object);
            userManagerMock.Setup(x => x.DeleteAsync(_testUser)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(_testUser, _userRole.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(_testUser, _adminRole.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(_testUser, _userRole.Name)).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.RemoveFromRoleAsync(_testUser, _adminRole.Name)).ReturnsAsync(IdentityResult.Success);
            
            return userManagerMock;
        }

        private IMapper CreateMapper()
        {
            return new MapperConfiguration(c => c.AddProfile<UserProfileMap>()).CreateMapper();
        }

        private List<IdentityRole> GetRoles()
        {
            return new List<IdentityRole>() { _userRole, _adminRole };
        }

        private Mock<RoleManager<IdentityRole>> CreateRoleManagerMock()
        {
            Mock<RoleManager<IdentityRole>> roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                null,
                null,
                null,
                null);

            roleManagerMock.Setup(x => x.Roles).Returns(GetRoles().AsQueryable().BuildMock().Object);

            return roleManagerMock;
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
            bool result = false;

            //Act

            updateResult = await userServices.Update(userModelToUpdate);

            if(updateResult.AreRolesUpdated == true && updateResult.IsPasswordUpdated == false)
            {
                result = true;
            }

            //Assert
            Assert.True(result);
        }

    }
}
