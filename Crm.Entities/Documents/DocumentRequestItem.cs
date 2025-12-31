using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Documents
{
    /// <summary>
    /// Evrak talebinin kalemleri (örn: "KDV alış faturaları", "Banka ekstresi"...).
    /// </summary>
    public class DocumentRequestItem : TenantEntity
    {
        public Guid DocumentRequestId { get; set; }
        public DocumentRequest DocumentRequest { get; set; } = default!;

        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Name { get; set; } = default!;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Notes { get; set; }

        public bool IsRequired { get; set; } = true;

        public ICollection<DocumentSubmission> Submissions { get; set; } = new List<DocumentSubmission>();
    }
}
