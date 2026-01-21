namespace Crm.Web.Models
{
    public class Personel
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Unvan { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Gorev { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public decimal Maas { get; set; }
        public string Durum { get; set; }
        public string Avatar { get; set; }
        public List<string> Yetkiler { get; set; }
    }

    public class PersonelViewModel
    {
        public List<Personel> Personeller { get; set; }
    }
}
