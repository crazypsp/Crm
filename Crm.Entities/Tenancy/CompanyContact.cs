using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Tenancy
{
    /// <summary>
    /// Firma yetkilileri/iletişim kişileri.
    /// </summary>
    public class CompanyContact : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        [Required, MaxLength(EntityConstants.NameMax)]
        public string Name { get; set; } = default!;

        [MaxLength(EntityConstants.EmailMax)]
        public string? Email { get; set; }

        [MaxLength(EntityConstants.PhoneMax)]
        public string? Phone { get; set; }

        [MaxLength(EntityConstants.NameMax)]
        public string? Title { get; set; }
    }
}
