﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Models;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Controllers
{
    public class LocalExtremeController : Controller
    {
        private readonly IExtremesServices _extremeServices;
        private readonly IDateChecker _dateChecker;
        private readonly ICurrenciesSelectList _currenciesSelectList;
        private readonly IDateRange _dateRange;
        public LocalExtremeController(IDateRange dateRange, IExtremesServices extremesServices, IDateChecker dateChecker, ICurrenciesSelectList currenciesSelectList)
        {
            _extremeServices = extremesServices;
            _dateChecker = dateChecker;
            _currenciesSelectList = currenciesSelectList;
            _dateRange = dateRange;
        }

        public async Task<IActionResult> FormOfLocalExtreme()
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowLocalExtreme(LocalExtremeValueModel model)
        {
            ViewData["currencyCodes"] = await _currenciesSelectList.GetCurrencyCodes(User.Identity.Name);
            if (!ModelState.IsValid)
            {
                return View("FormOfLocalExtreme", model);
            }
            if (! await _dateChecker.CheckIfDateExistInRange(model.StartDate, model.EndDate, model.NameCurrency))
            {
                ViewBag.DateRangeForLocalExtreme = await _dateRange.GetCurrencyDateRange(model.NameCurrency);

                return View("FormOfLocalExtreme", model);
            }
            return View(await _extremeServices.GetLocalExtremes(model));
        }
    }
}