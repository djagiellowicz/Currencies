using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WalutyBusinessLogic.LoadingFromFile;

namespace WalutyBusinessLogic.Services
{
    public interface IFavoritesService
    {
        Task<List<Currency>> GetLoggedUserFavCurrencies(ClaimsPrincipal user);
        Task<bool> AddFavCurrency(int currencyId, ClaimsPrincipal user);
        Task<bool> DeleteFavCurrency(int currencyId, ClaimsPrincipal user);
    }
}