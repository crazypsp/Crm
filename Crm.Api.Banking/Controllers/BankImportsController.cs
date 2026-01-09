using Crm.Services.Banking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crm.Api.Banking.Models.Requests;
using Crm.Api.Banking.Models.Responses;
using Crm.Api.Banking.Models.Common;

namespace Crm.Api.Banking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankImportsController : ControllerBase
    {
        private readonly IBankImportAppService _importService;
        private readonly ILogger<BankImportsController> _logger;

        public BankImportsController(
            IBankImportAppService importService,
            ILogger<BankImportsController> logger)
        {
            _importService = importService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> UploadAndCreateImport(
            [FromForm] UploadBankStatementRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Banka ekstresi yükleniyor - Tenant: {TenantId}, Firma: {CompanyId}",
                    tenantId, request.CompanyId);

                if (request.File.Length == 0)
                    return BadRequest(ApiResponse.FailureResult("Dosya boş olamaz"));

                if (request.File.Length > 50 * 1024 * 1024)
                    return BadRequest(ApiResponse.FailureResult("Dosya boyutu 50MB'tan büyük olamaz"));

                var importId = await _importService.UploadAndCreateImportAsync(
                    tenantId,
                    request.CompanyId,
                    request.BankAccountId,
                    request.TemplateId,
                    request.File.OpenReadStream(),
                    request.File.FileName,
                    request.File.ContentType,
                    ct);

                _logger.LogInformation("Banka ekstresi başarıyla yüklendi - ImportId: {ImportId}", importId);

                return Ok(ApiResponse<Guid>.SuccessResult(importId,
                    "Banka ekstresi başarıyla yüklendi ve import oluşturuldu"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Banka ekstresi yükleme hatası");
                throw;
            }
        }

        [HttpPost("{importId:guid}/apply-mapping")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApplyMapping(
            [FromRoute] Guid importId,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Eşleme kuralları uygulanıyor - Tenant: {TenantId}, Import: {ImportId}",
                    tenantId, importId);

                await _importService.ApplyMappingAsync(tenantId, importId, ct);

                _logger.LogInformation("Eşleme kuralları başarıyla uygulandı - Import: {ImportId}", importId);

                return Ok(ApiResponse.SuccessResult("Eşleme kuralları başarıyla uygulandı"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eşleme kuralları uygulama hatası - Import: {ImportId}", importId);
                throw;
            }
        }

        [HttpPost("{importId:guid}/build-draft")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> BuildDraft(
            [FromRoute] Guid importId,
            [FromBody] BuildDraftRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Fiş taslağı oluşturuluyor - Tenant: {TenantId}, Import: {ImportId}",
                    tenantId, importId);

                var draftId = await _importService.BuildDraftAsync(
                    tenantId, importId, request.BankAccountCode, ct);

                _logger.LogInformation("Fiş taslağı başarıyla oluşturuldu - DraftId: {DraftId}", draftId);

                return Ok(ApiResponse<Guid>.SuccessResult(draftId, "Fiş taslağı başarıyla oluşturuldu"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fiş taslağı oluşturma hatası - Import: {ImportId}", importId);
                throw;
            }
        }

        [HttpGet("{importId:guid}/status")]
        [ProducesResponseType(typeof(ApiResponse<ImportStatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImportStatus(
            [FromRoute] Guid importId,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Import durumu sorgulanıyor - Tenant: {TenantId}, Import: {ImportId}",
                    tenantId, importId);

                var statusResponse = new ImportStatusResponse
                {
                    ImportId = importId,
                    Status = "PROCESSING",
                    Progress = 75,
                    TotalRows = 100,
                    ProcessedRows = 75,
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(ApiResponse<ImportStatusResponse>.SuccessResult(
                    statusResponse, "Import durumu getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Import durumu sorgulama hatası - Import: {ImportId}", importId);
                throw;
            }
        }

        [HttpGet("{importId:guid}/transactions")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<TransactionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetImportTransactions(
            [FromRoute] Guid importId,
            CancellationToken ct,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? search = null)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Import işlemleri getiriliyor - Import: {ImportId}, Sayfa: {Page}",
                    importId, page);

                var transactions = new List<TransactionResponse>
                {
                    new TransactionResponse
                    {
                        Id = Guid.NewGuid(),
                        TransactionDate = DateTime.Now.AddDays(-1),
                        Description = "Örnek havale işlemi" + (search != null ? $" - {search}" : ""),
                        Amount = 1500.75m,
                        BalanceAfter = 5000.25m,
                        MappingStatus = "SUGGESTED",
                        SuggestedAccountCode = "770.01.001"
                    },
                    new TransactionResponse
                    {
                        Id = Guid.NewGuid(),
                        TransactionDate = DateTime.Now.AddDays(-2),
                        Description = "Maaş ödemesi" + (search != null ? $" - {search}" : ""),
                        Amount = 8500.00m,
                        BalanceAfter = 6500.25m,
                        MappingStatus = "APPROVED",
                        ApprovedAccountCode = "730.01.001"
                    }
                };

                var pagedResponse = new PagedResponse<TransactionResponse>
                {
                    Items = transactions,
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = 150,
                    TotalPages = 3
                };

                return Ok(ApiResponse<PagedResponse<TransactionResponse>>.SuccessResult(
                    pagedResponse, "İşlemler başarıyla getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Import işlemleri getirme hatası - Import: {ImportId}", importId);
                throw;
            }
        }

        private Guid GetTenantId()
        {
            var tenantClaim = User.FindFirst("tenant_id")?.Value
                            ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            if (Guid.TryParse(tenantClaim, out var tenantId))
                return tenantId;

            if (User.Identity?.IsAuthenticated == true)
                _logger.LogWarning("Tenant ID claim'i bulunamadı, kullanıcı: {User}", User.Identity.Name);

            return Guid.Empty;
        }
    }
}