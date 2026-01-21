namespace Crm.Web.Models
{
    public class Rapor
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Tur { get; set; }
        public DateTime Tarih { get; set; }
        public string Olusturan { get; set; }
        public string Boyut { get; set; }
        public string Format { get; set; }
    }

    public class RaporViewModel
    {
        public List<Rapor> Raporlar { get; set; }
    }
}
