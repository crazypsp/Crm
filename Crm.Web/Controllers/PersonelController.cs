using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class PersonelController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public PersonelController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            // Sadece Mali Müşavir personel görebilir
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "MaliMüşavir")
                return RedirectToAction("AccessDenied", "Home");

            ViewBag.JsonData = _mockDataService.GetPersonellerJson();
            return View();
        }
    }
}