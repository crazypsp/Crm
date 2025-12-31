using System.ComponentModel.DataAnnotations;
using Crm.Entities.Banking;
using Crm.Entities.Common;
using Crm.Entities.Documents;
using Crm.Entities.Integration;
using Crm.Entities.Messaging;
using Crm.Entities.Work;

namespace Crm.Entities.Tenancy
{
    /// <summary>
    /// Mükellef / Firma.
    /// Tenant'a bağlıdır.
    /// </summary>
    public class Company : TenantEntity
    {
        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Title { get; set; } = default!;

        [MaxLength(EntityConstants.CodeMax)]
        public string? TaxOffice { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? TaxNo { get; set; }

        [MaxLength(EntityConstants.EmailMax)]
        public string? Email { get; set; }

        [MaxLength(EntityConstants.PhoneMax)]
        public string? Phone { get; set; }

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<CompanyContact> Contacts { get; set; } = new List<CompanyContact>();
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public ICollection<IntegrationProfile> IntegrationProfiles { get; set; } = new List<IntegrationProfile>();

        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
        public ICollection<DocumentRequest> DocumentRequests { get; set; } = new List<DocumentRequest>();
        public ICollection<MessageThread> Threads { get; set; } = new List<MessageThread>();
    }
}
