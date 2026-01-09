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
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;

        public RolesController(ILogger<RolesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<RoleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Rol listesi getiriliyor");

                var roles = new List<RoleResponse>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "SuperAdmin",
                        Description = "Sistem yöneticisi - Tüm yetkilere sahip",
                        IsSystemRole = true,
                        UserCount = 1,
                        Permissions = new List<string>
                        {
                            "users.manage", "tenants.manage", "companies.manage",
                            "roles.manage", "system.manage", "audit.view"
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        Description = "Tenant yöneticisi - Kendi tenant'ını yönetir",
                        IsSystemRole = false,
                        UserCount = 5,
                        Permissions = new List<string>
                        {
                            "users.manage", "companies.manage", "reports.view"
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Editor",
                        Description = "Editör - İçerik yönetimi",
                        IsSystemRole = false,
                        UserCount = 12,
                        Permissions = new List<string>
                        {
                            "documents.create", "documents.edit", "reports.view"
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Viewer",
                        Description = "Görüntüleyici - Sadece görüntüleme",
                        IsSystemRole = false,
                        UserCount = 25,
                        Permissions = new List<string> { "reports.view" }
                    }
                };

                return Ok(ApiResponse<List<RoleResponse>>.SuccessResult(roles, "Rol listesi getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol listesi getirme hatası");
                throw;
            }
        }

        [HttpGet("{name}")]
        [ProducesResponseType(typeof(ApiResponse<RoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRole(
            [FromRoute] string name,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Rol getiriliyor: {RoleName}", name);

                // TODO: Business katmanına GetRoleAsync metodu eklenmeli
                await Task.Delay(100, ct);

                var role = new RoleResponse
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Description = name switch
                    {
                        "SuperAdmin" => "Sistem yöneticisi",
                        "Admin" => "Tenant yöneticisi",
                        "Editor" => "Editör",
                        "Viewer" => "Görüntüleyici",
                        _ => "Özel rol"
                    },
                    IsSystemRole = name is "SuperAdmin" or "Admin",
                    UserCount = name switch
                    {
                        "SuperAdmin" => 1,
                        "Admin" => 5,
                        "Editor" => 12,
                        "Viewer" => 25,
                        _ => 3
                    },
                    Permissions = new List<string>
                    {
                        "users.view",
                        name switch
                        {
                            "SuperAdmin" => "users.manage",
                            "Admin" => "users.manage",
                            _ => "users.view"
                        }
                    }
                };

                return Ok(ApiResponse<RoleResponse>.SuccessResult(role, "Rol bilgileri getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol getirme hatası: {RoleName}", name);
                throw;
            }
        }

        [HttpPost("{userId:guid}/assign-role")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRole(
            [FromRoute] Guid userId,
            [FromBody] AssignRoleRequest request,
            CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Rol atanıyor - Kullanıcı: {UserId}, Rol: {RoleName}",
                    userId, request.RoleName);

                // TODO: Business katmanına AssignRoleAsync metodu eklenmeli
                await Task.Delay(100, ct);

                return Ok(ApiResponse.SuccessResult($"'{request.RoleName}' rolü kullanıcıya başarıyla atandı"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol atama hatası - Kullanıcı: {UserId}", userId);
                throw;
            }
        }
    }
}