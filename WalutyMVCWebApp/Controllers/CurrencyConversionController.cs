using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class CurrencyConversionController : Controller
    {
        private readonly ICurrencyConversionService _currencyConversionService;
        private readonly IDateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly ICurrencyNameChecker _currencyNameChecker;
        private readonly ICurrenciesSelectList _currenciesSelectList;

        public CurrencyConversionController(IDateRange dateRange, IDateChecker dateChecker
                                            ,ICurrencyConversionService currencyConversionService, ICurrencyNameChecker currencyNameChecker, ICurrenciesSelectList currenciesSelectList)
        {
            _currencyConversionService = currencyConversionService;
            _dateChecker = dateChecker;
            _dateRange = dateRange;
            _currencyNameChecker = currencyNameChecker;
            _currenciesSelectList = currenciesSelectList;
        }

        public async Task<IActionResult> FormOfCurrencyConversion()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowResultCurrencyConversion(CurrencyConversionModel model)
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View("FormOfCurrencyConversion", model);
            }
            if (!_currencyNameChecker.AreDifferent(model.FirstCurrency, model.SecondCurrency))
            {
                ViewBag.NameErrorInfo = "Currencies name must different";
                return View("FormOfCurrencyConversion", model);
            }
            if (!(await _dateChecker.CheckIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrency, model.SecondCurrency)))
            {
                ViewBag.CommonDateRangeInfo = await _dateRange.GetCommonDateRangeForTwoCurrencies(model.FirstCurrency, model.SecondCurrency);

                return View("FormOfCurrencyConversion", model);
            }
            return View(await _currencyConversionService.CalculateCurrencyConversionAmount(model));
        }
    }
}