using Crm.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Integration
{
    /// <summary>
    /// Desktop Agent'ın bağlı olduğu bilgisayar kaydı.
    /// </summary>
    public class AgentMachine : TenantEntity
    {
        [Required, MaxLength(EntityConstants.NameMax)]
        public string MachineName { get; set; } = default!;

        [MaxLength(EntityConstants.NameMax)]
        public string? UserName { get; set; }

        [MaxLength(EntityConstants.CodeMax)]
        public string? AgentVersion { get; set; }

        public DateTimeOffset LastSeenAt { get; set; } = DateTimeOffset.UtcNow;

        public bool IsOnline { get; set; }
    }
}
