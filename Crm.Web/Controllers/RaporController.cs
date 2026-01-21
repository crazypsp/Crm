using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class RaporController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public RaporController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            ViewBag.JsonData = _mockDataService.GetRaporlarJson();
            return View();
        }
    }
}