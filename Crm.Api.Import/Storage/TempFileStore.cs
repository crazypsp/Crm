namespace Crm.Api.Import.Storage
{
  
        public interface ITempFileStore
        {
            Task<Guid> SaveAsync(Stream content, CancellationToken ct);
            Task<Stream> OpenReadAsync(Guid id, CancellationToken ct);
            Task DeleteAsync(Guid id, CancellationToken ct);
        }

        public sealed class LocalTempFileStore : ITempFileStore
        {
            private readonly string _root;

            public LocalTempFileStore(IWebHostEnvironment env)
            {
                _root = Path.Combine(env.ContentRootPath, "import-temp");
                Directory.CreateDirectory(_root);
            }

            public async Task<Guid> SaveAsync(Stream content, CancellationToken ct)
            {
                var id = Guid.NewGuid();
                var path = Path.Combine(_root, $"{id:N}.bin");

                await using var fs = File.Create(path);
                await content.CopyToAsync(fs, ct);

                return id;
            }

            public Task<Stream> OpenReadAsync(Guid id, CancellationToken ct)
            {
                var path = Path.Combine(_root, $"{id:N}.bin");
                if (!File.Exists(path))
                    throw new FileNotFoundException("Temp file not found.", path);

                Stream s = File.OpenRead(path);
                return Task.FromResult(s);
            }

            public Task DeleteAsync(Guid id, CancellationToken ct)
            {
                var path = Path.Combine(_root, $"{id:N}.bin");
                if (File.Exists(path))
                    File.Delete(path);

                return Task.CompletedTask;
            }
        }
    
}
