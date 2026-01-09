using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class ApproveMappingRequest
    {
        [Required(ErrorMessage = "Karşı hesap kodu zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Hesap kodu 3-50 karakter arasında olmalıdır")]
        public string CounterAccountCode { get; set; } = string.Empty;
    }
}