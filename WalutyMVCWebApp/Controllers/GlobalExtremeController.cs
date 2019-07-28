using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Services;
using WalutyBusinessLogic.Models;

namespace WalutyMVCWebApp.Controllers
{
    public class GlobalExtremeController : Controller
    {
        private readonly IExtremesServices _extremeServices;
        private readonly ICurrenciesSelectList _currenciesSelectList;

        public GlobalExtremeController(IExtremesServices extremesServices, ICurrenciesSelectList currenciesSelectList)
        {
            _extremeServices = extremesServices;
            _currenciesSelectList = currenciesSelectList;
        }

        public async Task<IActionResult> FormOfGlobalExtreme()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowGlobalExtreme(GlobalExtremeValueModel model)
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View("FormOfGlobalExtreme", model);
            }
            return View(await _extremeServices.GetGlobalExtreme(model));
        }
    }
}