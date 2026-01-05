namespace Crm.Api.Import.Contracts;

/// <summary>
/// Neden: Import dosyasının türünü parser seçimi için standardize eder.
/// </summary>
public enum StatementFileType
{
    Excel = 0,
    Pdf = 1
}

/// <summary>
/// Neden: Banka hareketinde para yönünü normalize eder.
/// Debit: çıkış, Credit: giriş.
/// </summary>
public enum MoneyDirection
{
    Debit = 0,
    Credit = 1
}

/// <summary>
/// Neden: Preview endpointi dosyayı commit etmeden satırları gösterir.
/// TemplateId DB’de NOT NULL olduğu için burada zorunlu tuttuk.
/// (Entity: BankStatementImport.TemplateId zorunlu)
/// </summary>
public sealed class PreviewBankStatementRequest
{
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid TemplateId { get; set; } // <-- Guid? değil, zorunlu
}

/// <summary>
/// Neden: Kullanıcıya satırları gösterir. Commit için TempFileId döner.
/// </summary>
public sealed class PreviewBankStatementResponse
{
    public Guid TempFileId { get; set; }
    public int TotalRows { get; set; }
    public IReadOnlyList<PreviewBankStatementRow> Rows { get; set; } = Array.Empty<PreviewBankStatementRow>();
}

public sealed class PreviewBankStatementRow
{
    public int RowNo { get; set; }
    public DateOnly TxDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public MoneyDirection Direction { get; set; }
    public decimal BalanceAfter { get; set; }
}

/// <summary>
/// Neden: Commit aşamasında DB’ye yazım yapılır. TemplateId zorunlu.
/// </summary>
public sealed class CommitBankStatementRequest
{
    public Guid TenantId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid TemplateId { get; set; } // <-- Guid? değil, zorunlu
    public Guid TempFileId { get; set; }
}

public sealed class CommitBankStatementResponse
{
    public Guid ImportId { get; set; }
    public int ImportedRows { get; set; }
}
