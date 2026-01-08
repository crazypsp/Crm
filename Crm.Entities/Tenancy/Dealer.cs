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

        // Hiyerarşik ilişki: Bir bayi bir üst bayiye sahip olabilir (opsiyonel)
        public Guid? ParentDealerId { get; set; }
        public Dealer? ParentDealer { get; set; }

        // Bir bayi birden çok alt bayiye sahip olabilir
        public ICollection<Dealer> SubDealers { get; set; } = new List<Dealer>();

        // Bir bayi birden çok tenant (mali müşavir ofisi) yönetebilir
        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
    }
}