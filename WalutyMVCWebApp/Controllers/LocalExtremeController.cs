using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class LocalExtremeController : Controller
    {
        private readonly IExtremesServices _extremeServices;
        private readonly DateChecker _dateChecker;
        private readonly DateRange _dateRange;
        public LocalExtremeController(ILoader loader, IExtremesServices extremesServices)
        {
            _extremeServices = extremesServices;
            _dateChecker = new DateChecker();
            _dateRange = new DateRange(loader);
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
            if (!_dateChecker.CheckingIfDateExistInRange(model.StartDate, model.EndDate, model.NameCurrency))
            {
                ViewBag.DateRangeForLocalExtreme = _dateRange.GetDateRangeCurrency(model.NameCurrency);

                return View("FormOfLocalExtreme", model);
            }
            return View(await _extremeServices.GetLocalExtremes(model));
        }
    }
}