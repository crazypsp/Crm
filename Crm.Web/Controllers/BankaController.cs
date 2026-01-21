using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class BankaController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public BankaController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            ViewBag.JsonData = _mockDataService.GetBankaHareketleriJson();
            return View();
        }

        [HttpPost]
        public JsonResult GetHareketler()
        {
            var data = _mockDataService.GetBankaHareketleriJson();
            return Json(data);
        }
    }
}