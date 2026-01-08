using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Documents
{
    /// <summary>
    /// Dosya metadatası. Dosyanın kendisi storage’da tutulur.
    /// </summary>
    public class DocumentFile : TenantEntity
    {
        [Required, MaxLength(EntityConstants.FileNameMax)]
        public string FileName { get; set; } = default!;
        [Required, MaxLength(EntityConstants.ContentTypeMax)]
        public string ContentType { get; set; } = default!;
        public long SizeBytes { get; set; }
        [Required, MaxLength(EntityConstants.StorageProviderMax)]
        public string StorageProvider { get; set; } = "local";
        [Required, MaxLength(EntityConstants.StoragePathMax)]
        public string StoragePath { get; set; } = default!;
        [MaxLength(EntityConstants.CodeMax)]
        public string? Sha256 { get; set; }

        // HEDEF 5: Mali yıl ve ay bazlı klasör yapısı
        public int? FiscalYear { get; set; }
        public int? FiscalMonth { get; set; }
        [MaxLength(EntityConstants.StoragePathMax)]
        public string? AutoFolderPath { get; set; }
    }
}
