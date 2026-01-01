namespace Crm.Api.Gateway.Contracts.Banking
{
    public sealed class TenantScopeRequest
    {
        public Guid TenantId { get; set; }
    }

    public sealed class UploadImportRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid TemplateId { get; set; }
        public IFormFile File { get; set; } = default!;
    }

    public sealed class ApproveTransactionRequest
    {
        public Guid TenantId { get; set; }
        public string CounterAccountCode { get; set; } = default!;
    }

    public sealed class BuildDraftRequest
    {
        public Guid TenantId { get; set; }
        public string BankAccountCode { get; set; } = default!;
    }

    public sealed class DispatchVoucherRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid IntegrationProfileId { get; set; }
        public Guid VoucherDraftId { get; set; }
    }

    public sealed class BankImportDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid TemplateId { get; set; }
        public Guid SourceFileId { get; set; }
        public int Status { get; set; }
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class BankTransactionDto
    {
        public Guid Id { get; set; }
        public int RowNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? ValueDate { get; set; }
        public string Description { get; set; } = default!;
        public string? ReferenceNo { get; set; }

        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }

        public int MappingStatus { get; set; }
        public string? SuggestedCounterAccountCode { get; set; }
        public string? ApprovedCounterAccountCode { get; set; }
        public decimal? Confidence { get; set; }
    }
}
