namespace Crm.Api.Documents.Storage
{
    public interface IFileStorage
    {
        /// <summary>
        /// Neden: Firma klasörü + yıl + ay klasörü standardı zorunlu.
        /// period => dosyanın ait olduğu dönem (yyyy/mm).
        /// </summary>
        Task<FileSaveResult> SaveAsync(
            Guid tenantId,
            Guid companyId,
            IFormFile file,
            DateTime period,
            CancellationToken ct);
    }

    public sealed class FileSaveResult
    {
        public string RelativePath { get; init; } = default!;
        public long SizeBytes { get; init; }
        public string FileName { get; init; } = default!;
        public string ContentType { get; init; } = default!;
        public string Sha256 { get; init; } = default!;
    }
}
