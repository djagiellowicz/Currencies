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
            // Not the best / optimal solution, better look for different way, but for now it's sufficient

            var users =  _userManager.Users;
            IList<UserDTO> listOfUsers = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                listOfUsers.Add(new UserDTO { Email = user.Email, Roles = roles });
            }

            return await listOfUsers.ToPagedListAsync(pageNumber, pageSize);
        }

    }
}
