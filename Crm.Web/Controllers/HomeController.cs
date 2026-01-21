using Crm.Web.Models;
using Crm.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Crm.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMockDataService _mockDataService;

        public HomeController(IMockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // JSON verilerinden kullanýcý kontrolü
                var kullanicilarJson = _mockDataService.GetKullanicilarJson();
                var kullanicilar = JsonSerializer.Deserialize<List<Kullanici>>(kullanicilarJson);

                var kullanici = kullanicilar?.FirstOrDefault(k =>
                    k.KullaniciAdi == model.KullaniciAdi &&
                    k.Sifre == model.Sifre &&
                    k.Aktif);

                if (kullanici != null)
                {
                    // Session'a bilgileri kaydet
                    HttpContext.Session.SetString("KullaniciId", kullanici.Id.ToString());
                    HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
                    HttpContext.Session.SetString("AdSoyad", kullanici.AdSoyad);
                    HttpContext.Session.SetString("Rol", kullanici.Rol);
                    HttpContext.Session.SetString("Avatar", kullanici.Avatar);

                    if (kullanici.FirmaId.HasValue)
                    {
                        HttpContext.Session.SetString("FirmaId", kullanici.FirmaId.Value.ToString());
                    }

                    return RedirectToAction("Index", "Dashboard");
                }

                ModelState.AddModelError("", "Kullanýcý adý veya þifre hatalý!");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CheckSession()
        {
            var kullaniciId = HttpContext.Session.GetString("KullaniciId");
            return Json(new { loggedIn = !string.IsNullOrEmpty(kullaniciId) });
        }
    }
}