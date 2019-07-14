using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.CurrenciesComparision;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class CurrencyComparisionController : Controller
    {
        private readonly CurrenciesComparator _currenciesComparator;
        private readonly DateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly CurrencyNameChecker _currencyNameChecker;

        public CurrencyComparisionController(ILoader loader, IDateRange dateRange)
        {
            _currenciesComparator = new CurrenciesComparator(loader);
            _dateChecker = new DateChecker();
            _dateRange = dateRange;
            _currencyNameChecker = new CurrencyNameChecker();
        }

        public IActionResult FormOfCurrencyComparator()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShowResultCurrencyComparision(CurrenciesComparatorModel model)
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
            if (!_dateChecker.CheckingIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.DateRangeForComparison = _dateRange.GetDateRangeTwoCurrencies(model.FirstCurrencyCode, model.SecondCurrencyCode);
                
                return View("FormOfCurrencyComparator", model);
            }
            return View(_currenciesComparator.CompareCurrencies(model));
        }
    }
}