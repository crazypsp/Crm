using Crm.Api.Documents.Contracts;
using Crm.Api.Documents.Infrastructure;
using Crm.Api.Documents.Storage;
using Crm.Data;
using Crm.Entities.Documents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Api.Documents.Controllers
{
    [ApiController]
    [Route("api/documents/submissions")]
    public sealed class SubmissionsController : ControllerBase
    {
        private readonly CrmDbContext _db;
        private readonly IFileStorage _storage;

        public SubmissionsController(CrmDbContext db, IFileStorage storage)
        {
            _db = db;
            _storage = storage;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(50_000_000)]
        public async Task<ActionResult<UploadSubmissionResponse>> Upload(
            [FromForm] Guid tenantId,
            [FromForm] Guid companyId,
            [FromForm] Guid? requestId,
            [FromForm] int? year,
            [FromForm] int? month,
            [FromForm] IFormFile file,
            CancellationToken ct)
        {
            // Neden: Dönem klasörlemesi için yıl/ay bilgisi gerekir. UI vermezse mevcut ayı kullanırız.
            var now = DateTime.UtcNow;
            var y = year ?? now.Year;
            var m = month ?? now.Month;

            if (m < 1 || m > 12)
                return BadRequest("month 1..12 arasında olmalıdır.");

            var period = new DateTime(y, m, 1);

            // Request doğrulama (opsiyonel)
            if (requestId is not null)
            {
                var exists = await _db.DocumentRequests.AsNoTracking()
                    .AnyAsync(x => x.TenantId == tenantId && x.Id == requestId && !x.IsDeleted, ct);

                if (!exists) return BadRequest("RequestId bulunamadı.");
            }

            // Storage'a kaydet: Tenant/Company/YYYY/MM hiyerarşisi burada uygulanır.
            var saved = await _storage.SaveAsync(tenantId, companyId, file, period, ct);

            // DocumentFile metadata
            var docFile = new DocumentFile
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(docFile,
                ("CompanyId", companyId),
                ("FileName", saved.FileName),
                ("OriginalFileName", saved.FileName),
                ("ContentType", saved.ContentType),
                ("SizeBytes", saved.SizeBytes),
                ("StoragePath", saved.RelativePath),
                ("Path", saved.RelativePath),
                ("Sha256", saved.Sha256),
                // Eğer entity destekliyorsa dönemi de yaz.
                ("Year", y),
                ("Month", m)
            );

            _db.DocumentFiles.Add(docFile);

            // Submission kaydı
            var submission = new DocumentSubmission
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            EntityMap.TrySet(submission,
                ("CompanyId", companyId),
                ("RequestId", requestId),
                ("DocumentRequestId", requestId),
                ("FileId", docFile.Id),
                ("DocumentFileId", docFile.Id),
                ("Status", 0),
                ("Year", y),
                ("Month", m)
            );

            _db.DocumentSubmissions.Add(submission);

            await _db.SaveChangesAsync(ct);

            return Ok(new UploadSubmissionResponse
            {
                DocumentFileId = docFile.Id,
                DocumentSubmissionId = submission.Id,
                RelativePath = saved.RelativePath
            });
        }
    }
}

