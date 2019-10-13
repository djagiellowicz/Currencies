using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using X.PagedList;

namespace WalutyBusinessLogic.Services
{
    class UserServices
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
            var listOfUsers = await _dbContext.Users
                .Select(x => _mapper.Map<User, UserDTO>(x))
                .ToPagedListAsync(pageNumber, pageSize);
            
            return listOfUsers;
        }

    }
}
