using Crm.Api.Import.Contracts;

namespace Crm.Api.Import.Parsing
{
    /// <summary>
    /// Neden: Excel/PDF farklı formatlar.
    /// Controller tek interface görür, DI ile uygun parser seçilir.
    /// </summary>
    public interface IBankStatementParser
    {
        StatementFileType FileType { get; }

        Task<IReadOnlyList<PreviewBankStatementRow>> ParseAsync(
            Stream fileStream,
            CancellationToken ct);
    }
}
