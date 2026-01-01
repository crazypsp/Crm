using Crm.Api.Import.Contracts;

namespace Crm.Api.Import.Parsing
{
    public interface IBankStatementParser
    {
        // Neden: Orchestrator dosya türünü otomatik seçer.
        Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct);
    }

    public interface IExcelBankStatementParser
    {
        Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct);
    }

    public interface IPdfBankStatementParser
    {
        Task<PreviewBankStatementResponse> PreviewAsync(IFormFile file, CancellationToken ct);
    }
}
