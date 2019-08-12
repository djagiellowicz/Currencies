using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly ICurrenciesSelectList _currenciesSelectList;
        private readonly ICurrencyRepository _currencyRepository;

        public ChartController(ICurrenciesSelectList currenciesSelectList, ICurrencyRepository currencyRepository)
        {
            _currenciesSelectList = currenciesSelectList;
            _currencyRepository = currencyRepository;
        }
        // GET: Chart
        public async Task<ActionResult> Index()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Details(string currencyCode)
        {
            var currency = await _currencyRepository.GetCurrency(currencyCode);
            var currencyRecords = currency.ListOfRecords;
            var model = new ChartModel(currencyCode, currencyRecords);

            return View("Details", model);
        }

    }
}