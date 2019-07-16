using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class LocalExtremeController : Controller
    {
        private readonly IExtremesServices _extremeServices;
        private readonly IDateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        public LocalExtremeController(IDateRange dateRange, IExtremesServices extremesServices, IDateChecker dateChecker)
        {
            _extremeServices = extremesServices;
            _dateChecker = dateChecker;
            _dateRange = dateRange;
        }

        public IActionResult FormOfLocalExtreme()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowLocalExtreme(LocalExtremeValueModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormOfLocalExtreme", model);
            }
            if (! await _dateChecker.CheckingIfDateExistInRange(model.StartDate, model.EndDate, model.NameCurrency))
            {
                ViewBag.DateRangeForLocalExtreme = await _dateRange.GetCurrencyDateRange(model.NameCurrency);

                return View("FormOfLocalExtreme", model);
            }
            return View(await _extremeServices.GetLocalExtremes(model));
        }
    }
}