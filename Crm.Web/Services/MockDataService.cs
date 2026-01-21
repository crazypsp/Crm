using System.Text.Json;

namespace Crm.Web.Services
{
    public interface IMockDataService
    {
        string GetKullanicilarJson();
        string GetMusterilerJson();
        string GetEvraklarJson();
        string GetBankaHareketleriJson();
        string GetIsTakipJson();
        string GetPersonellerJson();
        string GetDashboardJson();
        string GetRaporlarJson();
        string GetBildirimlerJson();
    }

    public class MockDataService : IMockDataService
    {
        public string GetKullanicilarJson()
        {
            var kullanicilar = new[]
            {
                new {
                    Id = 1,
                    KullaniciAdi = "admin",
                    Sifre = "123",
                    AdSoyad = "Ahmet Yılmaz",
                    Email = "ahmet@malimusavir.com",
                    Rol = "MaliMüşavir",
                    Avatar = "https://i.pravatar.cc/150?img=1",
                    Telefon = "0532 123 45 67",
                    Aktif = true,
                    UstPersonelId = (int?)null,
                    FirmaId = (int?)null
                },
                new {
                    Id = 2,
                    KullaniciAdi = "personel",
                    Sifre = "123",
                    AdSoyad = "Mehmet Demir",
                    Email = "mehmet@malimusavir.com",
                    Rol = "Personel",
                    Avatar = "https://i.pravatar.cc/150?img=2",
                    Telefon = "0532 234 56 78",
                    Aktif = true,
                    UstPersonelId = (int?)1,
                    FirmaId = (int?)null
                },
                new {
                    Id = 3,
                    KullaniciAdi = "firma1",
                    Sifre = "123",
                    AdSoyad = "Ali Kaya",
                    Email = "ali@abcstic.com",
                    Rol = "FirmaPersonel",
                    Avatar = "https://i.pravatar.cc/150?img=3",
                    Telefon = "0532 345 67 89",
                    Aktif = true,
                    UstPersonelId = (int?)null,
                    FirmaId = (int?)1
                }
            };

            return JsonSerializer.Serialize(kullanicilar);
        }

        public string GetMusterilerJson()
        {
            var musteriler = new[]
            {
                new {
                    Id = 1,
                    FirmaAdi = "ABC Ltd. Şti.",
                    VergiNo = "1234567890",
                    VergiDairesi = "İstanbul Vergi Dairesi",
                    Telefon = "0212 123 45 67",
                    Email = "info@abcstic.com",
                    Adres = "Maslak, İstanbul",
                    Sektor = "İnşaat",
                    MaliMusavirId = 1,
                    PersonelId = (int?)2,
                    Durum = "Aktif",
                    KayitTarihi = "2023-01-15",
                    SonEvrakTarihi = "2024-01-10",
                    Logo = "https://placehold.co/100x100/2c3e50/ffffff?text=ABC",
                    PersonelAdi = "Mehmet Demir"
                },
                new {
                    Id = 2,
                    FirmaAdi = "XYZ A.Ş.",
                    VergiNo = "0987654321",
                    VergiDairesi = "Ankara Vergi Dairesi",
                    Telefon = "0312 234 56 78",
                    Email = "info@xyz.com.tr",
                    Adres = "Çankaya, Ankara",
                    Sektor = "Teknoloji",
                    MaliMusavirId = 1,
                    PersonelId = (int?)2,
                    Durum = "Aktif",
                    KayitTarihi = "2023-03-20",
                    SonEvrakTarihi = "2024-01-12",
                    Logo = "https://placehold.co/100x100/3498db/ffffff?text=XYZ",
                    PersonelAdi = "Mehmet Demir"
                }
            };

            return JsonSerializer.Serialize(musteriler);
        }

        public string GetEvraklarJson()
        {
            var evraklar = new[]
            {
                new {
                    Id = 1,
                    FirmaId = 1,
                    FirmaAdi = "ABC Ltd. Şti.",
                    EvrakTuru = "Fatura",
                    EvrakNo = "FTR20240001",
                    Tarih = "2024-01-10",
                    Tutar = (decimal?)15000.00m,
                    Durum = "Onaylandı",
                    Yukleyen = "Ali Kaya",
                    YuklenmeTarihi = "2024-01-10 14:30",
                    Aciklama = "Ocak ayı danışmanlık faturası",
                    DosyaAdi = "fatura_ocak_2024.pdf",
                    DosyaBoyutu = "2.4 MB"
                },
                new {
                    Id = 2,
                    FirmaId = 1,
                    FirmaAdi = "ABC Ltd. Şti.",
                    EvrakTuru = "Banka Ekstresi",
                    EvrakNo = "BNK20240001",
                    Tarih = "2024-01-05",
                    Tutar = (decimal?)null,
                    Durum = "Beklemede",
                    Yukleyen = "Ali Kaya",
                    YuklenmeTarihi = "2024-01-08 10:15",
                    Aciklama = "Aralık ayı banka ekstresi",
                    DosyaAdi = "ekstre_aralik.pdf",
                    DosyaBoyutu = "1.8 MB"
                }
            };

            return JsonSerializer.Serialize(evraklar);
        }

        public string GetDashboardJson()
        {
            var dashboard = new
            {
                Istatistikler = new
                {
                    ToplamMusteri = 45,
                    AktifMusteri = 38,
                    BekleyenEvrak = 12,
                    BuAyTamamlananIs = 23,
                    ToplamCiro = 1250000.00,
                    AylikCiro = 185000.00
                },
                SonIslemler = new[]
                {
                    new {
                        Id = 1,
                        Tip = "evrak",
                        Aciklama = "ABC Ltd. yeni fatura yükledi",
                        Tarih = "10 dk önce",
                        Icon = "fa-file-invoice",
                        Color = "success"
                    },
                    new {
                        Id = 2,
                        Tip = "is",
                        Aciklama = "XYZ A.Ş. beyannamesi tamamlandı",
                        Tarih = "1 saat önce",
                        Icon = "fa-check-circle",
                        Color = "primary"
                    }
                },
                YaklasanSonTarihler = new[]
                {
                    new {
                        Id = 1,
                        FirmaAdi = "ABC Ltd. Şti.",
                        Islem = "KDV Beyannamesi",
                        SonTarih = "2024-01-26",
                        KalanGun = 2,
                        Oncelik = "yuksek"
                    },
                    new {
                        Id = 2,
                        FirmaAdi = "XYZ A.Ş.",
                        Islem = "Geçici Vergi",
                        SonTarih = "2024-01-28",
                        KalanGun = 4,
                        Oncelik = "orta"
                    }
                }
            };

            return JsonSerializer.Serialize(dashboard);
        }

        public string GetBankaHareketleriJson()
        {
            var hareketler = new[]
            {
                new {
                    Id = 1,
                    FirmaId = 1,
                    FirmaAdi = "ABC Ltd. Şti.",
                    HesapNo = "TR001234567890123456",
                    BankaAdi = "Ziraat Bankası",
                    IslemTarihi = "2024-01-10",
                    ValutarTarihi = "2024-01-10",
                    Aciklama = "Gelen Havale - ABC Ltd.",
                    Tutar = 15000.00m,
                    Bakiye = 45000.00m,
                    IslemTuru = "Gelen",
                    EvrakId = (int?)1,
                    Durum = "Eşleşti"
                }
            };

            return JsonSerializer.Serialize(hareketler);
        }

        public string GetIsTakipJson()
        {
            var isler = new[]
            {
                new {
                    Id = 1,
                    Baslik = "Aylık beyanname hazırlığı",
                    Aciklama = "ABC Ltd. için Ocak ayı KDV beyannamesinin hazırlanması",
                    FirmaId = 1,
                    FirmaAdi = "ABC Ltd. Şti.",
                    AtananPersonelId = 2,
                    AtananPersonel = "Mehmet Demir",
                    BaslangicTarihi = "2024-01-15",
                    BitisTarihi = "2024-01-20",
                    Durum = "Devam Ediyor",
                    Oncelik = "Yüksek",
                    TamamlanmaOrani = 60,
                    Kategori = "Beyanname"
                }
            };

            return JsonSerializer.Serialize(isler);
        }

        public string GetPersonellerJson()
        {
            var personeller = new[]
            {
                new {
                    Id = 1,
                    AdSoyad = "Ahmet Yılmaz",
                    Unvan = "Mali Müşavir",
                    Email = "ahmet@malimusavir.com",
                    Telefon = "0532 123 45 67",
                    Gorev = "Genel Müdür",
                    BaslangicTarihi = "2020-01-01",
                    Maas = 25000.00m,
                    Durum = "Aktif",
                    Avatar = "https://i.pravatar.cc/150?img=1",
                    Yetkiler = new string[] { "Tüm Yetkiler" }
                }
            };

            return JsonSerializer.Serialize(personeller);
        }

        public string GetRaporlarJson()
        {
            var raporlar = new[]
            {
                new {
                    Id = 1,
                    Adi = "Aylık Ciro Raporu",
                    Tur = "Finansal",
                    Tarih = "2024-01-15",
                    Olusturan = "Ahmet Yılmaz",
                    Boyut = "2.3 MB",
                    Format = "PDF"
                }
            };

            return JsonSerializer.Serialize(raporlar);
        }

        public string GetBildirimlerJson()
        {
            var bildirimler = new[]
            {
                new {
                    Id = 1,
                    Mesaj = "3 yeni evrak onayınız bekliyor",
                    Tip = "warning",
                    Tarih = "2024-01-15 14:30",
                    Okundu = false
                }
            };

            return JsonSerializer.Serialize(bildirimler);
        }
    }
}