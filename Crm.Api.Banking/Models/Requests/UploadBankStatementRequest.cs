using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class UploadBankStatementRequest
    {
        [Required(ErrorMessage = "Firma ID zorunludur")]
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "Banka hesap ID zorunludur")]
        public Guid BankAccountId { get; set; }

        [Required(ErrorMessage = "Şablon ID zorunludur")]
        public Guid TemplateId { get; set; }

        [Required(ErrorMessage = "Dosya zorunludur")]
        public IFormFile File { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Description { get; set; }
    }
}