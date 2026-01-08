using Crm.Business.Common;
using Crm.Data;
using Crm.Entities.Documents;
using Microsoft.EntityFrameworkCore;

namespace Crm.Business.Documents
{
    public sealed class DocumentRequestManager : IDocumentRequestManager
    {
        private readonly CrmDbContext _db;

        public DocumentRequestManager(CrmDbContext db)
        {
            _db = db;
        }

        public async Task<DocumentRequest> CreateRequestAsync(
            Guid tenantId,
            Guid companyId,
            string title,
            string? description,
            DateTimeOffset? dueAt,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(companyId, nameof(companyId));
            Guard.NotBlank(title, nameof(title));

            var companyOk = await _db.Companies
                .AnyAsync(x => x.Id == companyId && x.TenantId == tenantId && !x.IsDeleted, ct);
            if (!companyOk)
                throw new ForbiddenException("Firma bulunamadı veya tenant yetkisi yok.");

            var request = new DocumentRequest
            {
                TenantId = tenantId,
                CompanyId = companyId,
                Title = title.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                DueAt = dueAt
            };

            _db.DocumentRequests.Add(request);
            await _db.SaveChangesAsync(ct);

            return request;
        }

        public async Task<DocumentRequestItem> AddItemAsync(
            Guid tenantId,
            Guid requestId,
            string name,
            bool required,
            string? notes,
            CancellationToken ct)
        {
            Guard.NotEmpty(tenantId, nameof(tenantId));
            Guard.NotEmpty(requestId, nameof(requestId));
            Guard.NotBlank(name, nameof(name));

            var request = await _db.DocumentRequests
                .FirstOrDefaultAsync(x => x.Id == requestId && x.TenantId == tenantId && !x.IsDeleted, ct)
                ?? throw new NotFoundException("Evrak talebi bulunamadı.");

            var item = new DocumentRequestItem
            {
                TenantId = tenantId,
                DocumentRequestId = request.Id,
                Name = name.Trim(),
                IsRequired = required,
                Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim()
            };

            _db.DocumentRequestItems.Add(item);
            await _db.SaveChangesAsync(ct);

            return item;
        }
    }
}
