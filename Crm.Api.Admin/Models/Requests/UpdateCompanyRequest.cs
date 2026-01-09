using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Admin.Models.Requests
{
    public class UpdateCompanyRequest
    {
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Firma adı 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [StringLength(50, ErrorMessage = "Vergi numarası en fazla 50 karakter olabilir")]
        public string? TaxNo { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string? Phone { get; set; }

        [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string? Address { get; set; }

        public bool? IsActive { get; set; }
    }
}