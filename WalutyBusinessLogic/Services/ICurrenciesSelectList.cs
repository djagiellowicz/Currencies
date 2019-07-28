using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WalutyBusinessLogic.Services
{
    public interface ICurrenciesSelectList
    {
        Task<IEnumerable<SelectListItem>> GetCurrencyCodes(string identityName);
    }
}