using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class IsTakipController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public IsTakipController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            ViewBag.JsonData = _mockDataService.GetIsTakipJson();
            return View();
        }
    }
}