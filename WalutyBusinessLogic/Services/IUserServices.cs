using System.Threading.Tasks;
using WalutyBusinessLogic.Models.DTO;
using X.PagedList;

namespace WalutyBusinessLogic.Services
{
    public interface IUserServices
    {
        Task<IPagedList<UserDTO>> GetUsersPage(int pageNumber, int pageSize);
        Task<bool> Delete(string id);
        Task<DetailedUserDTO> GetUser(string id);
    }
}