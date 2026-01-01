namespace Crm.Api.Messaging.Contracts
{
    public sealed class CreateThreadRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }

        // Neden: Konu/başlık arama ve liste ekranlarında hızlı anlaşılır bir metin sağlar.
        public string Subject { get; set; } = default!;

        // Neden: Thread açılırken mali müşavir personelleri katılımcı olarak eklenir.
        public List<Guid> ParticipantUserIds { get; set; } = new();

        // Neden: Thread açılır açılmaz ilk mesaj gönderilebilsin.
        public string? InitialMessage { get; set; }

        // Neden: Audit ve “kim açtı?” için ilk mesajı atan kullanıcı bilgisi.
        public Guid? InitialSenderUserId { get; set; }
    }

    /// <summary>
    /// Base DTO (sealed OLMAMALI)
    /// Neden: ThreadDetailsDto bu sınıftan türeyip ortak alanları reuse edecek.
    /// </summary>
    public class ThreadDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }

        public string? Subject { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastMessageAt { get; set; }

        public int ParticipantsCount { get; set; }
    }

    /// <summary>
    /// Detay DTO: base alanlara ek olarak katılımcı listesini de taşır.
    /// </summary>
    public sealed class ThreadDetailsDto : ThreadDto
    {
        public List<Guid> ParticipantUserIds { get; set; } = new();
    }

    public sealed class SendMessageRequest
    {
        public Guid TenantId { get; set; }
        public Guid ThreadId { get; set; }

        // Neden: Audit ve “kim yazdı?” için zorunlu.
        public Guid SenderUserId { get; set; }

        public string Body { get; set; } = default!;

        // Neden: Personel iç notu / firma mesajı ayrımı gibi ihtiyaçlara temel.
        public bool IsInternal { get; set; } = false;
    }

    public sealed class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ThreadId { get; set; }
        public Guid SenderUserId { get; set; }
        public string? Body { get; set; }
        public bool? IsInternal { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class AddParticipantsRequest
    {
        public Guid TenantId { get; set; }
        public Guid ThreadId { get; set; }
        public List<Guid> UserIds { get; set; } = new();
    }

    public sealed class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
