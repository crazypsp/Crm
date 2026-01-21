namespace Crm.Web.Models
{
    public class MusteriModel
    {
        public int Id { get; set; }
        public string FirmaAdi { get; set; }
        public string VergiNo { get; set; }
        public string VergiDairesi { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }
        public string Sektor { get; set; }
        public int MaliMusavirId { get; set; }
        public int? PersonelId { get; set; }
        public string Durum { get; set; }
        public string KayitTarihi { get; set; }
        public string SonEvrakTarihi { get; set; }
        public string Logo { get; set; }
        public string PersonelAdi { get; set; }
    }
}