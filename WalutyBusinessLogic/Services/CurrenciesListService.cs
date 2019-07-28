using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;

namespace WalutyBusinessLogic.Services
{
    public interface ICurrenciesListService
    {
        Task<List<string>> GetCurrencyCodes();
    }

    public class CurrenciesListService : ICurrenciesListService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICurrencyRepository _repository;
        private readonly SignInManager<User> _signInManager;

        public CurrenciesListService(UserManager<User> userManager, ICurrencyRepository repository, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _repository = repository;
            _signInManager = signInManager;
        }

        public async Task<List<string>> GetCurrencyCodes()
        {
            List<string> CodesToReturn = (await _repository.GetAllCurrencyInfo())
                .Select(x => x.Code)
                .OrderBy(x => x)
                .ToList();

            return CodesToReturn;
        }
    }
}
