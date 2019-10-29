using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using X.PagedList;

namespace WalutyBusinessLogic.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly WalutyDBContext _dbContext;
        private readonly IMapper _mapper;

        public UserServices(UserManager<User> userManager, WalutyDBContext walutyDBContext, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = walutyDBContext;
            _mapper = mapper;
        }

        public async Task<IPagedList<UserDTO>> GetUsersPage(int pageNumber, int pageSize)
        {
            IPagedList<User> usersPage =  _userManager.Users.ToPagedList(pageNumber, pageSize);
            int totalNumberOfUsers = _userManager.Users.Count();
            IList<UserDTO> userDTOs = new List<UserDTO>();

            foreach (var user in usersPage)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDTOs.Add(new UserDTO { Email = user.Email, Roles = roles, Id = user.Id });
            }

            var usersDTOPagedList = new StaticPagedList<UserDTO>(userDTOs, pageNumber, pageSize, totalNumberOfUsers);

            return usersDTOPagedList;
        }

        public async Task<bool> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
               IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Update(UserPasswordModel userPasswordModel)
        {
            User user;
            IdentityResult result = new IdentityResult();

            if (userPasswordModel.Password == userPasswordModel.ConfirmPassword)
            {
               user = await _userManager.FindByIdAsync(userPasswordModel.UserId);

                if (user != null)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userPasswordModel.Password);
                    result = await _userManager.UpdateAsync(user);
                }

                if (result.Succeeded)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
