namespace Crm.Web.Models
{
    public class BankaHareketi
    {
        public int Id { get; set; }
        public int FirmaId { get; set; }
        public string FirmaAdi { get; set; }
        public string HesapNo { get; set; }
        public string BankaAdi { get; set; }
        public DateTime IslemTarihi { get; set; }
        public DateTime ValutarTarihi { get; set; }
        public string Aciklama { get; set; }
        public decimal Tutar { get; set; }
        public decimal Bakiye { get; set; }
        public string IslemTuru { get; set; }
        public int? EvrakId { get; set; }
        public string Durum { get; set; }
        public string TutarClass
        {
            get => Tutar >= 0 ? "text-success" : "text-danger";
        }
        public string IslemTuruIcon
        {
            get => IslemTuru == "Gelen" ? "fa-arrow-down" : "fa-arrow-up";
        }
    }

    public class BankaViewModel
    {
        public List<BankaHareketi> Hareketler { get; set; }
        public List<MusteriModel> Musteriler { get; set; }
        public decimal ToplamGelen { get; set; }
        public decimal ToplamGiden { get; set; }
        public decimal NetBakiye { get; set; }
    }
}
