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
    public class TenantsController : ControllerBase
    {
        private readonly ITenancyManager _tenancyManager;
        private readonly ILogger<TenantsController> _logger;

        public TenantsController(
            ITenancyManager tenancyManager,
            ILogger<TenantsController> logger)
        {
            _tenancyManager = tenancyManager;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<TenantResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTenant(
            [FromBody] CreateTenantRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Yeni tenant oluşturuluyor: {OfficeName}", request.OfficeName);

                var tenant = await _tenancyManager.CreateTenantAsync(
                    request.DealerId,
                    request.OfficeName,
                    ct);

                var response = new TenantResponse
                {
                    Id = tenant.Id,
                    DealerId = tenant.DealerId,
                    OfficeName = tenant.OfficeName,
                    IsActive = tenant.IsActive,
                    CreatedAt = tenant.CreatedAt.DateTime,
                    UpdatedAt = tenant.UpdatedAt.GetValueOrDefault().DateTime
                };

                return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id },
                    ApiResponse<TenantResponse>.SuccessResult(response, "Tenant başarıyla oluşturuldu"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tenant oluşturma hatası");
                throw;
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<TenantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTenant(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Tenant getiriliyor: {TenantId}", id);

                var tenant = await _tenancyManager.GetTenantAsync(id, ct);

                var response = new TenantResponse
                {
                    Id = tenant.Id,
                    DealerId = tenant.DealerId,
                    OfficeName = tenant.OfficeName,
                    IsActive = tenant.IsActive,
                    CreatedAt = tenant.CreatedAt.DateTime,
                    UpdatedAt = tenant.UpdatedAt.GetValueOrDefault().DateTime
                };

                return Ok(ApiResponse<TenantResponse>.SuccessResult(response, "Tenant bilgileri getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tenant getirme hatası: {TenantId}", id);
                throw;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<TenantResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTenants(
            [FromQuery] SearchRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Tenant listesi getiriliyor - Sayfa: {Page}, SayfaBoyutu: {PageSize}",
                    request.Page, request.PageSize);

                // TODO: Business katmanına GetTenantsAsync metodu eklenmeli
                // Şimdilik örnek veri dönüyoruz
                var tenants = new List<TenantResponse>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        DealerId = Guid.NewGuid(),
                        OfficeName = "Örnek Mali Müşavirlik",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddMonths(-6),
                        UpdatedAt = DateTime.UtcNow.AddDays(-10),
                        CompanyCount = 5,
                        UserCount = 12
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        DealerId = Guid.NewGuid(),
                        OfficeName = "Test Danışmanlık",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        UpdatedAt = DateTime.UtcNow.AddDays(-5),
                        CompanyCount = 3,
                        UserCount = 8
                    }
                };

                var pagedResponse = new PagedResponse<TenantResponse>
                {
                    Items = tenants,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = 25,
                    TotalPages = 2
                };

                return Ok(ApiResponse<PagedResponse<TenantResponse>>.SuccessResult(
                    pagedResponse, "Tenant listesi getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tenant listesi getirme hatası");
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "SuperAdmin")]
        [ProducesResponseType(typeof(ApiResponse<TenantResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTenant(
            [FromRoute] Guid id,
            [FromBody] UpdateTenantRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Tenant güncelleniyor: {TenantId}", id);

                // TODO: Business katmanına UpdateTenantAsync metodu eklenmeli
                // Şimdilik örnek işlem
                await Task.Delay(100, ct);

                var response = new TenantResponse
                {
                    Id = id,
                    DealerId = Guid.NewGuid(),
                    OfficeName = request.OfficeName ?? "Güncellenmiş Ofis",
                    IsActive = request.IsActive ?? true,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UpdatedAt = DateTime.UtcNow
                };

                return Ok(ApiResponse<TenantResponse>.SuccessResult(response, "Tenant başarıyla güncellendi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tenant güncelleme hatası: {TenantId}", id);
                throw;
            }
        }
    }
}