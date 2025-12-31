
namespace Crm.Entities.Common
{
    /// <summary>
    /// Tenant sınırı olan tüm tablolarda TenantId zorunlu.
    /// Tenant = Mali müşavir ofisi.
    /// </summary>
    public abstract class TenantEntity : BaseEntity
    {
        public Guid TenantId { get; set; }
    }
}
