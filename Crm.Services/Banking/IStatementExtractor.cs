using Crm.Entities.Contracts.Banking;

namespace Crm.Services.Banking
{
    public interface IStatementExtractor
    {
        /// <summary>
        /// Dosyadan ham satırları çıkarır (kolon adı -> değer).
        /// </summary>
        Task<ExtractResult> ExtractAsync(Stream file, string fileName, CancellationToken ct);
    }
}
