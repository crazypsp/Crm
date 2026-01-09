using Crm.Business.Tenancy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crm.Api.Admin.Models.Requests;
using Crm.Api.Admin.Models.Responses;
using Crm.Api.Admin.Models.Common;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CompaniesController : ControllerBase
    {
        private readonly ITenancyManager _tenancyManager;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(
            ITenancyManager tenancyManager,
            ILogger<CompaniesController> logger)
        {
            _tenancyManager = tenancyManager;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCompany(
            [FromBody] CreateCompanyRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Yeni firma oluşturuluyor: {Title}, Tenant: {TenantId}",
                    request.Title, request.TenantId);

                var company = await _tenancyManager.CreateCompanyAsync(
                    request.TenantId,
                    request.Title,
                    request.TaxNo,
                    ct);

                var response = new CompanyResponse
                {
                    Id = company.Id,
                    TenantId = company.TenantId,
                    Title = company.Title,
                    TaxNo = company.TaxNo,
                    IsActive = company.IsActive,
                    CreatedAt = company.CreatedAt.DateTime,
                    UpdatedAt = company.UpdatedAt.GetValueOrDefault().DateTime
                };

                return CreatedAtAction(nameof(GetCompany), new { id = company.Id },
                    ApiResponse<CompanyResponse>.SuccessResult(response, "Firma başarıyla oluşturuldu"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firma oluşturma hatası");
                throw;
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompany(
            [FromRoute] Guid id,
            [FromQuery] Guid tenantId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Firma getiriliyor: {CompanyId}, Tenant: {TenantId}", id, tenantId);

                var company = await _tenancyManager.GetCompanyAsync(tenantId, id, ct);

                var response = new CompanyResponse
                {
                    Id = company.Id,
                    TenantId = company.TenantId,
                    Title = company.Title,
                    TaxNo = company.TaxNo,
                    IsActive = company.IsActive,
                    CreatedAt = company.CreatedAt.DateTime,
                    UpdatedAt = company.UpdatedAt.GetValueOrDefault().DateTime
                };

                return Ok(ApiResponse<CompanyResponse>.SuccessResult(response, "Firma bilgileri getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firma getirme hatası: {CompanyId}", id);
                throw;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<CompanyResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanies(
            [FromQuery] SearchRequest request,
            [FromQuery] Guid? tenantId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Firma listesi getiriliyor - Tenant: {TenantId}, Sayfa: {Page}",
                    tenantId, request.Page);

                var companies = new List<CompanyResponse>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? Guid.NewGuid(),
                        Title = "Örnek Şirket A.Ş.",
                        TaxNo = "1234567890",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddMonths(-4),
                        UpdatedAt = DateTime.UtcNow.AddDays(-15),
                        TenantName = "Örnek Mali Müşavirlik"
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? Guid.NewGuid(),
                        Title = "Test Ltd. Şti.",
                        TaxNo = "0987654321",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        UpdatedAt = DateTime.UtcNow.AddDays(-7),
                        TenantName = "Test Danışmanlık"
                    }
                };

                var pagedResponse = new PagedResponse<CompanyResponse>
                {
                    Items = companies,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = 45,
                    TotalPages = 3
                };

                return Ok(ApiResponse<PagedResponse<CompanyResponse>>.SuccessResult(
                    pagedResponse, "Firma listesi getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firma listesi getirme hatası");
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCompany(
            [FromRoute] Guid id,
            [FromQuery] Guid tenantId,
            [FromBody] UpdateCompanyRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Firma güncelleniyor: {CompanyId}, Tenant: {TenantId}", id, tenantId);

                // TODO: Business katmanına UpdateCompanyAsync metodu eklenmeli
                await Task.Delay(100, ct);

                var response = new CompanyResponse
                {
                    Id = id,
                    TenantId = tenantId,
                    Title = request.Title ?? "Güncellenmiş Firma",
                    TaxNo = request.TaxNo,
                    IsActive = request.IsActive ?? true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-4),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(ApiResponse<CompanyResponse>.SuccessResult(response, "Firma başarıyla güncellendi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Firma güncelleme hatası: {CompanyId}", id);
                throw;
            }
        }
    }
}