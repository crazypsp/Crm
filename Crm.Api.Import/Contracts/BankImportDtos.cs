namespace Crm.Api.Import.Contracts
{
    public sealed class BankStatementRowDto
    {
        // Neden: Kaynak format ne olursa olsun tek modelde normalize ediyoruz.
        public int RowNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = default!;
        public decimal Debit { get; set; }   // Borç
        public decimal Credit { get; set; }  // Alacak
        public decimal Balance { get; set; } // Son bakiye
    }

    public sealed class PreviewBankStatementResponse
    {
        public string DetectedFormat { get; set; } = default!; // excel/pdf/unknown
        public List<string> Warnings { get; set; } = new();
        public List<BankStatementRowDto> Rows { get; set; } = new();
    }

    public sealed class CommitBankStatementResponse
    {
        // Neden: Banking modülü bu ImportId ile “fiş üretme”ye geçer.
        public Guid ImportId { get; set; }
        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }
    }
}
