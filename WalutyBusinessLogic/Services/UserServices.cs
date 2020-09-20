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
        private readonly IMapper _mapper;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserServices(UserManager<User> userManager, IMapper mapper, IPasswordValidator<User> passwordValidator, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _passwordValidator = passwordValidator;
            _roleManager = roleManager;
    }
        public async Task<UserDTO> GetUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            UserDTO userDTO = _mapper.Map<User, UserDTO>(user);

            userDTO.Roles = await _userManager.GetRolesAsync(user);

            return userDTO;
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

        public async Task<UpdateUserResult> Update(UserModel model)
        {
            User user;
            IList<IdentityRole> results = new List<IdentityRole>(); 
            IdentityResult result;
            bool areRolesUpdated = true;
            bool isPasswordUpdated = false;
            List<IdentityRole> allRoles = _roleManager.Roles.Select(x => x).ToList();

            user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {

                if (model.Password == model.ConfirmPassword && model.Password != null && model.ConfirmPassword != null)
                {
                    result = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);

                    if (result.Succeeded)
                    {
                        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
                        result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            isPasswordUpdated = true;
                        }
                    }
                }

                if (model.NewRoles != null)
                {
                    foreach (var role in allRoles)
                    {
                        if (model.NewRoles.Contains(role.Name))
                        {
                            if (model.Roles.Contains(role.Name))
                            {

                            }
                            else
                            {
                                result = await _userManager.AddToRoleAsync(user, role.Name);

                                if (!result.Succeeded && areRolesUpdated != false)
                                {
                                    areRolesUpdated = false; 
                                }
                            }
                        }
                        else
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, role.Name);

                            if (!result.Succeeded && areRolesUpdated != false)
                            {
                                areRolesUpdated = false;
                            }
                        }
                    }
                }
                
            }
            return new UpdateUserResult(isPasswordUpdated, areRolesUpdated);
        }
    }
}
