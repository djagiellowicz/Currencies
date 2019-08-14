using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly ICurrenciesSelectList _currenciesSelectList;
        private readonly IChartService _chartService;

        public IChartService ChartService { get; }

        public ChartController(ICurrenciesSelectList currenciesSelectList, IChartService chartService)
        {
            _currenciesSelectList = currenciesSelectList;
            _chartService = chartService;

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
            var model = await _chartService.CreateChartModel(currencyCode);

            return View("Details", model);
        }

    }
}