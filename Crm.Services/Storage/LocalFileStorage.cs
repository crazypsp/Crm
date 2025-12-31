using Crm.Services.Common;
using Microsoft.Extensions.Options;

namespace Crm.Services.Storage
{
    public sealed class LocalFileStorage : IFileStorage
    {
        private readonly FileStorageOptions _opt;

        public LocalFileStorage(IOptions<FileStorageOptions> opt) => _opt = opt.Value;

        public async Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct)
        {
            try
            {
                Directory.CreateDirectory(_opt.RootPath);

                var safeName = $"{Guid.NewGuid():N}_{Path.GetFileName(fileName)}";
                var path = Path.Combine(_opt.RootPath, safeName);

                await using var fs = File.Create(path);
                await content.CopyToAsync(fs, ct);

                // DB’de saklayacağımız “storagePath” budur.
                return path;
            }
            catch (Exception ex)
            {
                throw new StorageException($"Dosya kaydedilemedi: {ex.Message}");
            }
        }

        public Task<Stream> OpenReadAsync(string storagePath, CancellationToken ct)
        {
            try
            {
                Stream s = File.OpenRead(storagePath);
                return Task.FromResult(s);
            }
            catch (Exception ex)
            {
                throw new StorageException($"Dosya açılamadı: {ex.Message}");
            }
        }
    }
}
