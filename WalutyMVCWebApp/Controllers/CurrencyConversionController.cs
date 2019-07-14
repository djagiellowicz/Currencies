using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class CurrencyConversionController : Controller
    {
        private readonly CurrencyConversionService _currencyConversionService;
        private readonly IDateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly CurrencyNameChecker _currencyNameChecker;
        public CurrencyConversionController(ILoader loader, IDateRange dateRange, IDateChecker dateChecker)
        {
            _currencyConversionService = new CurrencyConversionService(loader);
            _dateChecker = dateChecker;
            _dateRange = dateRange;
            _currencyNameChecker = new CurrencyNameChecker();
        }

        public IActionResult FormOfCurrencyConversion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> ShowResultCurrencyConversion(CurrencyConversionModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormOfCurrencyConversion", model);
            }
            if (!_currencyNameChecker.CheckingIfCurrencyNamesAreDifferent(model.FirstCurrency, model.SecondCurrency))
            {
                ViewBag.ResultChekingCurrencyNameInConversion = "Currencies name must different";
                return View("FormOfCurrencyConversion", model);
            }
            if (! await _dateChecker.CheckingIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrency, model.SecondCurrency))
            {
                ViewBag.DateRangeForConversion = _dateRange.GetDateRangeTwoCurrencies(model.FirstCurrency, model.SecondCurrency);

                return View("FormOfCurrencyConversion", model);
            }
            return View(_currencyConversionService.CalculateAmountForCurrencyConversion(model));
        }
    }
}