using Crm.Entities.Common;
using Crm.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Documents
{
    /// <summary>
    /// Firma/personel tarafından yüklenen evrak.
    /// </summary>
    public class DocumentSubmission : TenantEntity
    {
        public Guid DocumentRequestItemId { get; set; }
        public DocumentRequestItem DocumentRequestItem { get; set; } = default!;

        public Guid FileId { get; set; }
        public DocumentFile File { get; set; } = default!;

        public Guid UploadedByUserId { get; set; }
        public ApplicationUser UploadedByUser { get; set; } = default!;

        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Comment { get; set; }
    }
}
