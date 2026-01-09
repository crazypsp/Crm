using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Admin.Models.Requests
{
    public class UpdateTenantRequest
    {
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Ofis adı 3-200 karakter arasında olmalıdır")]
        public string? OfficeName { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string? ContactEmail { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string? ContactPhone { get; set; }

        public bool? IsActive { get; set; }
    }
}