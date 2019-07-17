using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WalutyBusinessLogic.Services;
using WalutyBusinessLogic.Models;

namespace WalutyMVCWebApp.Controllers
{
    public class GlobalExtremeController : Controller
    {
        private readonly IExtremesServices _extremeServices;
        public GlobalExtremeController(IExtremesServices extremesServices)
        {
            _extremeServices = extremesServices;
        }

        public IActionResult FormOfGlobalExtreme()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowGlobalExtreme(GlobalExtremeValueModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FormOfGlobalExtreme", model);
            }
            return View(await _extremeServices.GetGlobalExtreme(model));
        }
    }
}