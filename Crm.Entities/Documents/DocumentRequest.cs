using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Documents
{
    /// <summary>
    /// Firma için evrak talebi.
    /// </summary>
    public class DocumentRequest : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Title { get; set; } = default!;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Description { get; set; }

        public DocumentRequestStatus Status { get; set; } = DocumentRequestStatus.Requested;

        public DateTimeOffset? DueAt { get; set; }

        public ICollection<DocumentRequestItem> Items { get; set; } = new List<DocumentRequestItem>();
    }
}
