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

        public CurrencyComparisionController(ICurrenciesComparator currenciesComparator, IDateRange dateRange, IDateChecker dateChecker
                                            ,ICurrencyNameChecker currencyNameChecker)
        {
            _currenciesComparator = currenciesComparator;
            _dateChecker = dateChecker;
            _dateRange = dateRange;
            _currencyNameChecker = currencyNameChecker;
        }

        public IActionResult FormOfCurrencyComparator()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowResultCurrencyComparision(CurrenciesComparatorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormOfCurrencyComparator", model);
            }
            if (!_currencyNameChecker.CheckingIfCurrencyNamesAreDifferent(model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.ResultChekingCurrencyNameInComparision = "Currencies name must different";
                return View("FormOfCurrencyComparator", model);
            }
            if (!await _dateChecker.CheckingIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.DateRangeForComparison = await _dateRange.GetCommonDateRangeForTwoCurrencies(model.FirstCurrencyCode, model.SecondCurrencyCode);
                
                return View("FormOfCurrencyComparator", model);
            }
            return View(await _currenciesComparator.CompareCurrencies(model));
        }
    }
}