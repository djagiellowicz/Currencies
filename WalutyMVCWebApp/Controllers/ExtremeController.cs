﻿using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyMVCWebApp.Extremes;

namespace WalutyMVCWebApp.Controllers
{
    public class ExtremeController : Controller
    {
        private readonly ExtremesServices _extremeServices;

        public ExtremeController(ILoader loader)
        {
            _extremeServices = new ExtremesServices(loader);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GlobalExtreme(string nameCurrency)
        {
            return View(_extremeServices.GetGlobalExtremes(nameCurrency));
        }

        public IActionResult LocalExtreme()
        {
            return View();
        }
    }
}