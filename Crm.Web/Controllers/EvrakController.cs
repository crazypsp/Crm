using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Web.Controllers
{
    public class EvrakController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public EvrakController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            var model = new EvrakViewModel
            {
                JsonData = _mockDataService.GetEvraklarJson(),
                MusteriJson = _mockDataService.GetMusterilerJson()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Yukle()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            ViewBag.Musteriler = _mockDataService.GetMusterilerJson();
            return View();
        }

        [HttpPost]
        public JsonResult SaveEvrak([FromBody] dynamic evrak)
        {
            return Json(new { success = true, message = "Evrak kaydedildi (demo)" });
        }

        [HttpPost]
        public JsonResult GetEvraklar()
        {
            var data = _mockDataService.GetEvraklarJson();
            return Json(data);
        }
    }
}