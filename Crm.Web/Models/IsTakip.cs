namespace Crm.Web.Models
{
    public class IsTakip
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public int FirmaId { get; set; }
        public string FirmaAdi { get; set; }
        public int? AtananPersonelId { get; set; }
        public string AtananPersonel { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string Durum { get; set; }
        public string Oncelik { get; set; }
        public int TamamlanmaOrani { get; set; }
        public string Kategori { get; set; }
    }

    public class IsTakipViewModel
    {
        public List<IsTakip> IsTakipListesi { get; set; }
        public List<MusteriModel> Musteriler { get; set; }
        public List<Personel> Personeller { get; set; }
    }
}
