using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class CurrencyConversionController : Controller
    {
        private readonly CurrencyConversionService _currencyConversionService;
        private readonly DateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly CurrencyNameChecker _currencyNameChecker;
        public CurrencyConversionController(ILoader loader, IDateRange dateRange)
        {
            _currencyConversionService = new CurrencyConversionService(loader);
            _dateChecker = new DateChecker();
            _dateRange = dateRange;
            _currencyNameChecker = new CurrencyNameChecker();
        }

        public IActionResult FormOfCurrencyConversion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowResultCurrencyConversion(CurrencyConversionModel model)
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
            if (!_dateChecker.CheckingIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrency, model.SecondCurrency))
            {
                ViewBag.DateRangeForConversion = _dateRange.GetDateRangeTwoCurrencies(model.FirstCurrency, model.SecondCurrency);

                return View("FormOfCurrencyConversion", model);
            }
            return View(_currencyConversionService.CalculateAmountForCurrencyConversion(model));
        }
    }
}