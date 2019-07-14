using System.Threading.Tasks;
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
        private readonly IDateChecker _dateChecker;
        private readonly IDateRange _dateRange;
        private readonly CurrencyNameChecker _currencyNameChecker;

        public CurrencyComparisionController(ILoader loader, IDateRange dateRange, IDateChecker dateChecker)
        {
            _currenciesComparator = new CurrenciesComparator(loader);
            _dateChecker = dateChecker;
            _dateRange = dateRange;
            _currencyNameChecker = new CurrencyNameChecker();
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
            if (! await _dateChecker.CheckingIfDateExistsForTwoCurrencies(model.Date, model.FirstCurrencyCode, model.SecondCurrencyCode))
            {
                ViewBag.DateRangeForComparison = _dateRange.GetDateRangeTwoCurrencies(model.FirstCurrencyCode, model.SecondCurrencyCode);
                
                return View("FormOfCurrencyComparator", model);
            }
            return View(_currenciesComparator.CompareCurrencies(model));
        }
    }
}