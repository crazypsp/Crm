using Crm.Entities.Common;
using Crm.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Integration
{
    /// <summary>
    /// Agent'a gönderilen komutların kuyruk/sonuç takibi.
    /// </summary>
    public class IntegrationJob : TenantEntity
    {
        public Guid? CompanyId { get; set; }
        public Guid? AgentMachineId { get; set; }

        /// <summary>
        /// Örn: PostVoucher, PullChartOfAccounts, TestConnection
        /// </summary>
        [Required, MaxLength(EntityConstants.NameMax)]
        public string CommandType { get; set; } = default!;

        /// <summary>
        /// Command payload (JSON).
        /// </summary>
        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string PayloadJson { get; set; } = default!;

        public IntegrationJobStatus Status { get; set; } = IntegrationJobStatus.Queued;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? ResultJson { get; set; }

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? ErrorMessage { get; set; }

        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}
