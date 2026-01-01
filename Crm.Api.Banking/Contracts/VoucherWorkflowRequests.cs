using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Contracts
{
    /// <summary>
    /// Neden: Import (BankTransaction) -> tek fiş (VoucherDraft) üretmek için gereken parametreleri taşır.
    /// Import okuma işi Import API’de; muhasebe fişi üretme Banking API’de yapılır.
    /// </summary>
    public sealed class CreateVoucherDraftFromImportRequest
    {
        [Required] public Guid TenantId { get; set; }
        [Required] public Guid CompanyId { get; set; }
        [Required] public Guid ImportId { get; set; }

        /// <summary>
        /// Neden: Banka hesabı muhasebe kodu bazen BankAccount’tan gelir, bazen kullanıcı override eder.
        /// </summary>
        public string? BankGlAccountCodeOverride { get; set; }

        /// <summary>
        /// Neden: Eşleşmeyen satırlar için geçici/şüpheli hesap.
        /// FailIfUnmapped=false ise kullanılır.
        /// </summary>
        public string SuspenseAccountCode { get; set; } = "999.99";

        /// <summary>
        /// Neden: Unmapped satır varsa işlem dursun mu?
        /// </summary>
        public bool FailIfUnmapped { get; set; } = false;

        /// <summary>
        /// Neden: Kullanıcı fiş tarihini sabitlemek isteyebilir.
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        public string? HeaderDescription { get; set; }
    }

    /// <summary>
    /// Neden: Import satırında “onaylanan karşı hesap kodu” yazıp mapping’i kalıcı hale getirmek.
    /// ImportsQueryController satırlarda ApprovedCounterAccountCode döndürüyor. :contentReference[oaicite:1]{index=1}
    /// </summary>
    public sealed class ApproveTransactionCounterAccountRequest
    {
        [Required] public Guid TenantId { get; set; }
        [Required] public string ApprovedCounterAccountCode { get; set; } = default!;
    }

    /// <summary>
    /// Neden: ERP’ye yazma işlemi Agent tarafından yapılacak, bu yüzden job enqueue ediyoruz.
    /// </summary>
    public sealed class EnqueuePostVoucherRequest
    {
        [Required] public Guid TenantId { get; set; }
        public Guid? IntegrationProfileId { get; set; }
    }

    public sealed class EnqueuePostVoucherResponse
    {
        public Guid JobId { get; set; }
    }
}
