using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Tenancy
{
    /// <summary>
    /// Bayi: Mali müşavir ofislerini yönetir.
    /// </summary>
    public class Dealer : BaseEntity
    {
        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = default!;

        [MaxLength(EntityConstants.CodeMax)]
        public string? TaxNo { get; set; }

        [MaxLength(EntityConstants.PhoneMax)]
        public string? Phone { get; set; }

        [MaxLength(EntityConstants.EmailMax)]
        public string? Email { get; set; }

        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
}
