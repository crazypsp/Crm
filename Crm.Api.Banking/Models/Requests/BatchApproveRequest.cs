using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class BatchApproveRequest
    {
        [Required(ErrorMessage = "İşlem ID listesi zorunludur")]
        [MinLength(1, ErrorMessage = "En az bir işlem ID'si gereklidir")]
        public List<Guid> TransactionIds { get; set; } = new();

        [Required(ErrorMessage = "Hesap kodu zorunludur")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Hesap kodu 3-50 karakter arasında olmalıdır")]
        public string CounterAccountCode { get; set; } = string.Empty;
    }
}