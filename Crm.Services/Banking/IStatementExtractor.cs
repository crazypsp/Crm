using Crm.Entities.Contracts.Banking;

namespace Crm.Services.Banking
{
    public interface IStatementExtractor
    {
        Task<ExtractResult> ExtractAsync(Stream file, string fileName, CancellationToken ct);
    }
}