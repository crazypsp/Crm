using System.Security.Cryptography;

namespace Crm.Api.Import.Storage
{
    public sealed class LocalImportFileStorage : IImportFileStorage
    {
        private readonly string _root;

        public LocalImportFileStorage(string absoluteRoot)
        {
            // Neden: Uygulama ayağa kalkınca root garanti olsun.
            _root = Path.GetFullPath(absoluteRoot);
            Directory.CreateDirectory(_root);
        }

        public async Task<ImportFileSaveResult> SaveAsync(
            Guid tenantId,
            Guid companyId,
            IFormFile file,
            DateTime period,
            CancellationToken ct)
        {
            // Neden: Documents standardı: tenant/company/yyyy/mm
            var year = period.Year;
            var month = period.Month;

            var tenantFolder = Path.Combine(_root, tenantId.ToString("N"));
            var companyFolder = Path.Combine(tenantFolder, companyId.ToString("N"));
            Directory.CreateDirectory(companyFolder);

            var yearFolder = Path.Combine(companyFolder, year.ToString("0000"));
            var isNewYear = !Directory.Exists(yearFolder);
            Directory.CreateDirectory(yearFolder);

            // Neden: Yıl klasörü ilk kez oluştuğunda 12 ay klasörü otomatik açılsın.
            if (isNewYear)
            {
                for (int m = 1; m <= 12; m++)
                    Directory.CreateDirectory(Path.Combine(yearFolder, m.ToString("00")));
            }

            var monthFolder = Path.Combine(yearFolder, month.ToString("00"));
            Directory.CreateDirectory(monthFolder);

            // Neden: path traversal / güvenli dosya adı
            var safeOriginalName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(safeOriginalName);

            // Neden: Diskte çakışmasın diye guid isim
            var id = Guid.NewGuid();
            var fileNameOnDisk = $"{id:N}{ext}";
            var absolutePath = Path.Combine(monthFolder, fileNameOnDisk);

            // Neden: Hash (audit + duplicate tespiti + delil)
            using var sha = SHA256.Create();
            await using var outStream = File.Create(absolutePath);
            await using var inStream = file.OpenReadStream();

            var buffer = new byte[81920];
            int read;
            long total = 0;

            while ((read = await inStream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
            {
                sha.TransformBlock(buffer, 0, read, null, 0);
                await outStream.WriteAsync(buffer.AsMemory(0, read), ct);
                total += read;
            }
            sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

            var relativePath = Path.Combine(
                tenantId.ToString("N"),
                companyId.ToString("N"),
                year.ToString("0000"),
                month.ToString("00"),
                fileNameOnDisk
            ).Replace("\\", "/");

            return new ImportFileSaveResult
            {
                RelativePath = relativePath,
                FileNameOnDisk = fileNameOnDisk,
                OriginalFileName = safeOriginalName,
                ContentType = file.ContentType ?? "application/octet-stream",
                SizeBytes = total,
                Sha256 = Convert.ToHexString(sha.Hash!)
            };
        }
    }
}
