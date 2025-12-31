
namespace Crm.Services.Storage
{
    public interface IFileStorage
    {
        /// <summary>
        /// Dosyayı storage'a yazar, storagePath döner.
        /// </summary>
        Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct);

        /// <summary>
        /// StoragePath'ten stream açar.
        /// </summary>
        Task<Stream> OpenReadAsync(string storagePath, CancellationToken ct);
    }
}
