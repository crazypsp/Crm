namespace Crm.Services.Storage
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(Stream content, string fileName, CancellationToken ct);
        Task<Stream> OpenReadAsync(string storagePath, CancellationToken ct);
    }
}