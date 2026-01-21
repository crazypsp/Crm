using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public DashboardController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            var model = new DashboardViewModel
            {
                JsonData = _mockDataService.GetDashboardJson(),
                KullaniciAdi = HttpContext.Session.GetString("AdSoyad"),
                Rol = HttpContext.Session.GetString("Rol")
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult GetDashboardData()
        {
            var data = _mockDataService.GetDashboardJson();
            return Json(data);
        }
    }
}