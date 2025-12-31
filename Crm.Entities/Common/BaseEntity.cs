using System.ComponentModel.DataAnnotations;


namespace Crm.Entities.Common
{
    /// <summary>
    /// Tüm tablolarda ortak: PK + audit + soft delete + concurrency.
    /// </summary>
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public Guid? CreatedByUserId { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
        public Guid? UpdatedByUserId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public Guid? DeletedByUserId { get; set; }

        /// <summary>
        /// EF Concurrency Token olarak kullanılacak (Data katmanında IsRowVersion set edilecek).
        /// </summary>
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
