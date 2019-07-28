using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class CurrencyComparisionController : Controller
    {
        private readonly ICurrenciesComparator _currenciesComparator;
        private readonly IDateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly ICurrencyNameChecker _currencyNameChecker;
        private readonly ICurrenciesSelectList _currenciesSelectList;

        public CurrencyComparisionController(ICurrenciesComparator currenciesComparator, IDateRange dateRange, IDateChecker dateChecker
                                            ,ICurrencyNameChecker currencyNameChecker, ICurrenciesSelectList currenciesSelectList)
        {
            _currenciesComparator = currenciesComparator;
            _dateChecker = dateChecker;
            _dateRange = dateRange;
            _currencyNameChecker = currencyNameChecker;
            _currenciesSelectList = currenciesSelectList;
        }

        public async Task<IActionResult> FormOfCurrencyComparator()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowResultCurrencyComparision(CurrenciesComparatorModel model)
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes();

            if (!ModelState.IsValid)
            {
                return View("FormOfCurrencyComparator", model);
            }
            if (!_currencyNameChecker.AreDifferent(model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.NameErrorInfo = "Currencies name must different";
                return View("FormOfCurrencyComparator", model);
            }
            if (!await _dateChecker.CheckIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.CommonDateRangeInfo = await _dateRange.GetCommonDateRangeForTwoCurrencies(model.FirstCurrencyCode, model.SecondCurrencyCode);
                
                return View("FormOfCurrencyComparator", model);
            }

            return View(await _currenciesComparator.CompareCurrencies(model));
        }
    }
}