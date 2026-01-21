namespace Crm.Web.Models
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; } // MaliMüşavir, Personel, FirmaPersonel
        public string Avatar { get; set; }
        public string Telefon { get; set; }
        public bool Aktif { get; set; }
        public int? FirmaId { get; set; }
        public int? UstPersonelId { get; set; }
    }    
}
