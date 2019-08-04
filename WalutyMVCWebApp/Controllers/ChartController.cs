using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly ICurrenciesSelectList _currenciesSelectList;

        public ChartController(ICurrenciesSelectList currenciesSelectList)
        {
            _currenciesSelectList = currenciesSelectList;
        }
        // GET: Chart
        public async Task<ActionResult> Index()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult Details(int id)
        {
            return View();
        }

    }
}