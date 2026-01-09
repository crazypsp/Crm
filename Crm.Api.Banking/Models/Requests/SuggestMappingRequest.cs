using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class SuggestMappingRequest
    {
        [Required(ErrorMessage = "Önerilen hesap kodu zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Hesap kodu 3-50 karakter arasında olmalıdır")]
        public string SuggestedAccountCode { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Reason { get; set; }
    }
}