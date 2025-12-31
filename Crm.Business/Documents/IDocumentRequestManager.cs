using Crm.Entities.Documents;

namespace Crm.Business.Documents
{
    public interface IDocumentRequestManager
    {
        Task<DocumentRequest> CreateRequestAsync(Guid tenantId, Guid companyId, string title, string? description, DateTimeOffset? dueAt, CancellationToken ct);
        Task<DocumentRequestItem> AddItemAsync(Guid tenantId, Guid requestId, string name, bool required, string? notes, CancellationToken ct);
    }
}
