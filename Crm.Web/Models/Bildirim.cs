namespace Crm.Web.Models
{
    public class Bildirim
    {
        public int Id { get; set; }
        public string Mesaj { get; set; }
        public string Tip { get; set; }
        public DateTime Tarih { get; set; }
        public bool Okundu { get; set; }
    }
}
