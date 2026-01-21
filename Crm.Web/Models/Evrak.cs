namespace Crm.Web.Models
{
    public class Evrak
    {
        public int Id { get; set; }
        public int FirmaId { get; set; }
        public string FirmaAdi { get; set; }
        public string EvrakTuru { get; set; }
        public string EvrakNo { get; set; }
        public DateTime Tarih { get; set; }
        public decimal? Tutar { get; set; }
        public string Durum { get; set; }
        public string Yukleyen { get; set; }
        public DateTime YuklenmeTarihi { get; set; }
        public string Aciklama { get; set; }
        public string DosyaAdi { get; set; }
        public string DosyaBoyutu { get; set; }
        public string DosyaIkonu
        {
            get
            {
                if (DosyaAdi.EndsWith(".pdf")) return "fa-file-pdf";
                if (DosyaAdi.EndsWith(".xlsx") || DosyaAdi.EndsWith(".xls")) return "fa-file-excel";
                if (DosyaAdi.EndsWith(".doc") || DosyaAdi.EndsWith(".docx")) return "fa-file-word";
                return "fa-file";
            }
        }
    }

  
    public class EvrakFilter
    {
        public int? FirmaId { get; set; }
        public string Durum { get; set; }
        public string EvrakTuru { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
    }
}
