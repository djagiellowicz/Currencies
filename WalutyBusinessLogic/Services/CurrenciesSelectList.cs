using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{

    public class CurrenciesSelectList : ICurrenciesSelectList
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyRepository _repository;
        private readonly SignInManager<User> _signInManager;

        public CurrenciesSelectList(UserManager<User> userManager, ICurrencyRepository repository, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _repository = repository;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<SelectListItem>> GetCurrencyCodes()
        {
            var listItems = (await _repository.GetAllCurrencyInfo())
                .OrderBy(x => x.Code)
                .Select(x => new SelectListItem(x.Code, x.Code))
                .ToList();

            return listItems;
        }
    }
}
