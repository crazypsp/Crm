namespace Crm.Api.Banking.Contracts
{
    public sealed class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; } = new();
    }

    public sealed class BankImportListItemDto
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

    public sealed class BankTransactionListItemDto
    {
        public Guid Id { get; set; }
        public int RowNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = default!;
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
        public int MappingStatus { get; set; }
        public string? SuggestedCounterAccountCode { get; set; }
        public string? ApprovedCounterAccountCode { get; set; }
        public decimal? Confidence { get; set; }
    }
}
