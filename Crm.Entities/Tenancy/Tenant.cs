using System.ComponentModel.DataAnnotations;
using Crm.Entities.Common;
using Crm.Entities.Integration;

namespace Crm.Entities.Tenancy
{
    /// <summary>
    /// Tenant = Mali müşavir ofisi. CRM verilerinin sınırı.
    /// </summary>
    public class Tenant : BaseEntity
    {
        public Guid DealerId { get; set; }
        public Dealer Dealer { get; set; } = default!;

        [Required, MaxLength(EntityConstants.TitleMax)]
        public string OfficeName { get; set; } = default!;

        [MaxLength(EntityConstants.CodeMax)]
        public string? TaxOffice { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? TaxNo { get; set; }

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public ICollection<AgentMachine> AgentMachines { get; set; } = new List<AgentMachine>();
    }
}
