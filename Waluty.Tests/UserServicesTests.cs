using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

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

            Mock<IMapper> mapperMock = new Mock<IMapper>();
            Mock<IMapper> iMapperMock = new Mock<IMapper>();
            Mock<IPasswordValidator<User>> iPasswordValidatorMock = new Mock<IPasswordValidator<User>>();
            Mock<RoleManager<IdentityRole>> roleManagerMock = new Mock<RoleManager<IdentityRole>>();


            DbContextOptions contextOptions = new DbContextOptions<WalutyDBContext>();
            var options = new DbContextOptionsBuilder<WalutyDBContext>().UseInMemoryDatabase("test").Options;

            WalutyDBContext context = new WalutyDBContext(options);

            //Give proper user
            userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(new User());

            UserServices userServices = new UserServices(userManagerMock.Object,
                context,mapperMock.Object,
                iPasswordValidatorMock.Object,
                roleManagerMock.Object
                );
          



        }

    }
}
