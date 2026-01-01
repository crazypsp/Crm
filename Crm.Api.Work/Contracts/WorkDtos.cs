namespace Crm.Api.Work.Contracts
{
    public sealed class CreateTaskRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }

        // Neden: Görevin UI’daki temel adı.
        public string Title { get; set; } = default!;

        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        // Neden: MVP’de status int tutmak kolaydır; entity enum ise TrySet enum’a çevirir.
        public int Status { get; set; } = 0;

        public int? Priority { get; set; }

        // Neden: Görev oluşturulurken aynı anda personele atanabilsin.
        public List<Guid> AssigneeUserIds { get; set; } = new();
    }

    public sealed class UpdateTaskRequest
    {
        public Guid TenantId { get; set; }

        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public int Status { get; set; }
        public int? Priority { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed class TaskDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        public int? Status { get; set; }
        public int? Priority { get; set; }

        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        // Neden: “Kimlere atandı?” ekranı.
        public List<Guid> AssigneeUserIds { get; set; } = new();
    }

    public sealed class AssignUsersRequest
    {
        public Guid TenantId { get; set; }
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
