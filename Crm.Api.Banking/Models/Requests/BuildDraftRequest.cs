using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class BuildDraftRequest
    {
        [Required(ErrorMessage = "Banka hesap kodu zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Banka hesap kodu 3-50 karakter arasında olmalıdır")]
        public string BankAccountCode { get; set; } = string.Empty;
    }
}