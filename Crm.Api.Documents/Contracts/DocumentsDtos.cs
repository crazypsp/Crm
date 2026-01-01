namespace Crm.Api.Documents.Contracts
{
    public sealed class CreateDocumentRequestRequest
    {
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }

        // Neden: UI’da istek başlığı.
        public string Title { get; set; } = default!;

        public string? Notes { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        // Neden: Talep edilen evrak kalemleri.
        public List<CreateDocumentRequestItemDto> Items { get; set; } = new();
    }

    public sealed class CreateDocumentRequestItemDto
    {
        public string Name { get; set; } = default!;
        public bool IsRequired { get; set; } = true;
    }

    public sealed class DocumentRequestDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid CompanyId { get; set; }
        public string? Title { get; set; }
        public string? Notes { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public int? Status { get; set; }
        public int ItemsCount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class UploadSubmissionResponse
    {
        public Guid DocumentFileId { get; set; }
        public Guid DocumentSubmissionId { get; set; }

        // Neden: DB’ye ve UI’ya mutlak path değil “relative path” dönmek taşınabilirlik sağlar.
        public string RelativePath { get; set; } = default!;
    }
}
