using AutoMapper;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;

namespace WalutyBusinessLogic.AutoMapper.Profiles
{
    public class UserProfileMap : Profile
    {
        public UserProfileMap()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
