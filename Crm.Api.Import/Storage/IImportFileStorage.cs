namespace Crm.Api.Import.Storage
{
    public interface IImportFileStorage
    {
        // Neden: Dosyaları dönem bazlı klasörleriz (yyyy/mm).
        Task<ImportFileSaveResult> SaveAsync(
            Guid tenantId,
            Guid companyId,
            IFormFile file,
            DateTime period,
            CancellationToken ct);
    }

    public sealed class ImportFileSaveResult
    {
        // Neden: DB’de absolute path değil relative path tutmak taşınabilirlik sağlar.
        public string RelativePath { get; init; } = default!;
        public string FileNameOnDisk { get; init; } = default!;
        public string OriginalFileName { get; init; } = default!;
        public string ContentType { get; init; } = default!;
        public long SizeBytes { get; init; }
        public string Sha256 { get; init; } = default!;
    }
}
