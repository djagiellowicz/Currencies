using System.Threading.Tasks;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Models.DTO;
using X.PagedList;

namespace WalutyBusinessLogic.Services
{
    public interface IUserServices
    {
        Task<IPagedList<UserDTO>> GetUsersPage(int pageNumber, int pageSize);
        Task<bool> Delete(string id);
        Task<UpdateUserResult> Update(UserModel model);
        Task<UserDTO> GetUser(string id);
    }
}