using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Crm.Web.Controllers
{
    public class MusteriController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public MusteriController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        public IActionResult Index(string search = "", string durum = "", string sektor = "")
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciId))
                return RedirectToAction("Login", "Home");

            var model = new MusteriViewModel
            {
                JsonData = _mockDataService.GetMusterilerJson(),
                FiltreJson = JsonSerializer.Serialize(new { search, durum, sektor })
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult GetMusteriler()
        {
            var data = _mockDataService.GetMusterilerJson();
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetMusteriById(int id)
        {
            var musterilerJson = _mockDataService.GetMusterilerJson();
            var musteriler = JsonSerializer.Deserialize<List<dynamic>>(musterilerJson);
            var musteri = musteriler.FirstOrDefault(m => m.GetProperty("id").GetInt32() == id);

            return Json(musteri);
        }

        [HttpPost]
        public JsonResult SaveMusteri([FromBody] dynamic musteri)
        {
            // Bu örnekte sadece JSON döndürüyoruz
            return Json(new { success = true, message = "Müşteri kaydedildi (demo)" });
        }

        [HttpPost]
        public JsonResult DeleteMusteri(int id)
        {
            return Json(new { success = true, message = "Müşteri silindi (demo)" });
        }
    }
}