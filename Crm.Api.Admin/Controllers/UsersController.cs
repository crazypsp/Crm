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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Yeni kullanıcı oluşturuluyor: {Email}, Tenant: {TenantId}",
                    request.Email, request.TenantId);

                // TODO: Business katmanına CreateUserAsync metodu eklenmeli
                await Task.Delay(100, ct);

                var response = new UserResponse
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    Roles = request.Roles
                };

                return CreatedAtAction(nameof(GetUser), new { id = response.Id },
                    ApiResponse<UserResponse>.SuccessResult(response, "Kullanıcı başarıyla oluşturuldu"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma hatası");
                throw;
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(
            [FromRoute] Guid id,
            [FromQuery] Guid tenantId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Kullanıcı getiriliyor: {UserId}, Tenant: {TenantId}", id, tenantId);

                // TODO: Business katmanına GetUserAsync metodu eklenmeli
                await Task.Delay(100, ct);

                var response = new UserResponse
                {
                    Id = id,
                    TenantId = tenantId,
                    Email = "user@example.com",
                    FirstName = "Örnek",
                    LastName = "Kullanıcı",
                    IsActive = true,
                    LastLoginAt = DateTime.UtcNow.AddHours(-2),
                    CreatedAt = DateTime.UtcNow.AddMonths(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5),
                    Roles = new List<string> { "User", "Editor" }
                };

                return Ok(ApiResponse<UserResponse>.SuccessResult(response, "Kullanıcı bilgileri getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirme hatası: {UserId}", id);
                throw;
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(
            [FromQuery] SearchRequest request,
            [FromQuery] Guid? tenantId,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Kullanıcı listesi getiriliyor - Tenant: {TenantId}, Sayfa: {Page}",
                    tenantId, request.Page);

                var users = new List<UserResponse>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? Guid.NewGuid(),
                        Email = "admin@example.com",
                        FirstName = "Admin",
                        LastName = "User",
                        IsActive = true,
                        LastLoginAt = DateTime.UtcNow.AddHours(-1),
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        Roles = new List<string> { "Admin" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? Guid.NewGuid(),
                        Email = "editor@example.com",
                        FirstName = "Editor",
                        LastName = "User",
                        IsActive = true,
                        LastLoginAt = DateTime.UtcNow.AddDays(-1),
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        Roles = new List<string> { "Editor" }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId ?? Guid.NewGuid(),
                        Email = "viewer@example.com",
                        FirstName = "Viewer",
                        LastName = "User",
                        IsActive = true,
                        LastLoginAt = DateTime.UtcNow.AddDays(-3),
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        Roles = new List<string> { "Viewer" }
                    }
                };

                var pagedResponse = new PagedResponse<UserResponse>
                {
                    Items = users,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = 35,
                    TotalPages = 2
                };

                return Ok(ApiResponse<PagedResponse<UserResponse>>.SuccessResult(
                    pagedResponse, "Kullanıcı listesi getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı listesi getirme hatası");
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(
            [FromRoute] Guid id,
            [FromQuery] Guid tenantId,
            [FromBody] UpdateUserRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Kullanıcı güncelleniyor: {UserId}, Tenant: {TenantId}", id, tenantId);

                // TODO: Business katmanına UpdateUserAsync metodu eklenmeli
                await Task.Delay(100, ct);

                var response = new UserResponse
                {
                    Id = id,
                    TenantId = tenantId,
                    Email = request.Email ?? "updated@example.com",
                    FirstName = request.FirstName ?? "Güncellenmiş",
                    LastName = request.LastName ?? "Kullanıcı",
                    IsActive = request.IsActive ?? true,
                    LastLoginAt = DateTime.UtcNow.AddHours(-1),
                    CreatedAt = DateTime.UtcNow.AddMonths(-2),
                    UpdatedAt = DateTime.UtcNow,
                    Roles = request.Roles ?? new List<string> { "User" }
                };

                return Ok(ApiResponse<UserResponse>.SuccessResult(response, "Kullanıcı başarıyla güncellendi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme hatası: {UserId}", id);
                throw;
            }
        }

        [HttpPost("{id:guid}/reset-password")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ResetPassword(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Şifre sıfırlanıyor: {UserId}", id);

                // TODO: Business katmanına ResetPasswordAsync metodu eklenmeli
                await Task.Delay(100, ct);

                return Ok(ApiResponse.SuccessResult("Şifre başarıyla sıfırlandı. Yeni şifre kullanıcıya email ile gönderildi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre sıfırlama hatası: {UserId}", id);
                throw;
            }
        }
    }
}