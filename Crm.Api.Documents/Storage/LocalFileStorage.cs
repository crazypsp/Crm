using System.Security.Cryptography;

namespace Crm.Api.Documents.Storage
{
    public sealed class LocalFileStorage : IFileStorage
    {
        private readonly string _root;

        public LocalFileStorage(string root)
        {
            _root = Path.GetFullPath(root);
            Directory.CreateDirectory(_root);
        }

        public async Task<FileSaveResult> SaveAsync(
            Guid tenantId,
            Guid companyId,
            IFormFile file,
            DateTime period,
            CancellationToken ct)
        {
            // Neden: Döneme göre klasörleme (YYYY/MM) evrak takibinde kritik.
            var year = period.Year;
            var month = period.Month;

            // Tenant/Company kök klasörü: her firma için ayrı klasör.
            var companyRoot = Path.Combine(_root, tenantId.ToString("N"), companyId.ToString("N"));
            Directory.CreateDirectory(companyRoot);

            // Yıl klasörü
            var yearFolder = Path.Combine(companyRoot, year.ToString("0000"));
            var newYear = !Directory.Exists(yearFolder);
            Directory.CreateDirectory(yearFolder);

            // Kritik: yıl oluştuğunda 12 ay klasörü otomatik oluşturulsun.
            if (newYear)
            {
                for (var m = 1; m <= 12; m++)
                    Directory.CreateDirectory(Path.Combine(yearFolder, m.ToString("00")));
            }

            // Hedef ay klasörü
            var monthFolder = Path.Combine(yearFolder, month.ToString("00"));
            Directory.CreateDirectory(monthFolder);

            // Dosya güvenliği: sadece filename al (path traversal önlemi).
            var safeName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(safeName);

            // Diskte benzersiz isim: Guid + ext
            var id = Guid.NewGuid();
            var fileNameOnDisk = $"{id:N}{ext}";
            var absolutePath = Path.Combine(monthFolder, fileNameOnDisk);

            // Hash: bütünlük / tekrar yükleme tespiti.
            using var sha = SHA256.Create();

            await using var fs = File.Create(absolutePath);
            await using var input = file.OpenReadStream();

            var buffer = new byte[81920];
            int read;
            long total = 0;

            while ((read = await input.ReadAsync(buffer.AsMemory(0, buffer.Length), ct)) > 0)
            {
                sha.TransformBlock(buffer, 0, read, null, 0);
                await fs.WriteAsync(buffer.AsMemory(0, read), ct);
                total += read;
            }
            sha.TransformFinalBlock(Array.Empty<byte>(), 0, 0);

            // DB’ye mutlak path değil, relative path yazıyoruz (taşınabilirlik).
            var relative = Path.Combine(
                tenantId.ToString("N"),
                companyId.ToString("N"),
                year.ToString("0000"),
                month.ToString("00"),
                fileNameOnDisk
            ).Replace("\\", "/");

            return new FileSaveResult
            {
                RelativePath = relative,
                SizeBytes = total,
                FileName = safeName,
                ContentType = file.ContentType ?? "application/octet-stream",
                Sha256 = Convert.ToHexString(sha.Hash!)
            };
        }
    }
}
