using Microsoft.AspNetCore.Mvc;

namespace WalutyMVCWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}