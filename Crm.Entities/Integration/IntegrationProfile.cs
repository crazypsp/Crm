using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Integration
{
    /// <summary>
    /// ERP/DB entegrasyon profili.
    /// Tenant bazlı veya Company bazlı olabilir.
    /// </summary>
    public class IntegrationProfile : TenantEntity
    {
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        public ProgramType ProgramType { get; set; }

        public Guid SecretId { get; set; }
        public ConnectionSecret Secret { get; set; } = default!;

        [MaxLength(EntityConstants.CodeMax)]
        public string? BranchCode { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? WorkplaceCode { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
