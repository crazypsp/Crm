using Crm.Entities.Common;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;
using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Work
{
    /// <summary>
    /// Personel görevleri.
    /// </summary>
    public class WorkTask : TenantEntity
    {
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }

        [Required, MaxLength(EntityConstants.TitleMax)]
        public string Title { get; set; } = default!;

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Description { get; set; }

        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.Open;

        /// <summary>
        /// 1 yüksek öncelik, 5 düşük gibi bir ölçek.
        /// </summary>
        public int Priority { get; set; } = 3;

        public DateTimeOffset? DueAt { get; set; }

        public ICollection<WorkTaskAssignment> Assignments { get; set; } = new List<WorkTaskAssignment>();
    }
}
