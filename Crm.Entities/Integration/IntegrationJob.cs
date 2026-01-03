using Crm.Entities.Common;
using Crm.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Integration
{
    /// <summary>
    /// Agent kuyruğu: ERP/DB işlemleri doğrudan web’den yapılmaz; job olarak Agent’a gider.
    /// </summary>
    public class IntegrationJob : TenantEntity
    {
        /// <summary>
        /// Neden: İşin bağlı olduğu firma scope’u (raporlama/filtreleme ve yetki kontrolü için).
        /// </summary>
        public Guid? CompanyId { get; set; }

        /// <summary>
        /// Neden: İşin hedef agent makinesine pin’lenmesi istenebilir (özellikle local DB bağlantıları için).
        /// </summary>
        public Guid? AgentMachineId { get; set; }

        /// <summary>
        /// Neden: Agent’ın hangi executor’ı çalıştıracağını belirler (PostVoucherDraft vb.).
        /// Business katmanı da bu alanı kullanır.
        /// </summary>
        [Required, MaxLength(100)]
        public string CommandType { get; set; } = default!;

        /// <summary>
        /// Neden: İş parametreleri (voucherDraftId, integrationProfileId, vs.) JSON taşınır.
        /// </summary>
        [Required]
        public string PayloadJson { get; set; } = "{}";

        /// <summary>
        /// Neden: Agent iş sonucunu (ERP fiş no vb) geri yazabilir.
        /// </summary>
        public string? ResultJson { get; set; }

        /// <summary>
        /// Neden: Kuyruk yaşam döngüsü.
        /// </summary>
        public IntegrationJobStatus Status { get; set; } = (IntegrationJobStatus)0; // default: Queued

        /// <summary>
        /// Neden: Retry sayacı (max sonrası dead-letter).
        /// </summary>
        public int Attempts { get; set; }

        /// <summary>
        /// Neden: Lock mekanizması (aynı job iki agent’a verilmesin).
        /// </summary>
        [MaxLength(100)]
        public string? LockedBy { get; set; }

        public DateTimeOffset? LockedUntil { get; set; }

        /// <summary>
        /// Neden: Support/diagnostics.
        /// </summary>
        [MaxLength(2000)]
        public string? LastError { get; set; }
    }
}
