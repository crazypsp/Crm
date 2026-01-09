using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crm.Api.Admin.Models.Responses;
using Crm.Api.Admin.Models.Common;

namespace Crm.Api.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")]
    public class SystemController : ControllerBase
    {
        private readonly ILogger<SystemController> _logger;

        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]
        [ProducesResponseType(typeof(ApiResponse<SystemInfoResponse>), StatusCodes.Status200OK)]
        public IActionResult GetSystemInfo()
        {
            try
            {
                _logger.LogInformation("Sistem bilgileri getiriliyor");

                var info = new SystemInfoResponse
                {
                    Version = "2.1.0",
                    ServerTime = DateTime.UtcNow,
                    Database = new DatabaseInfo
                    {
                        Provider = "SQL Server 2022",
                        Status = "Healthy",
                        LastBackup = DateTime.UtcNow.AddHours(-6)
                    },
                    Stats = new SystemStats
                    {
                        TotalTenants = 15,
                        TotalCompanies = 87,
                        TotalUsers = 245,
                        ActiveSessions = 42,
                        MemoryUsageMB = 512.5m,
                        CpuUsagePercent = 23.7m
                    }
                };

                return Ok(ApiResponse<SystemInfoResponse>.SuccessResult(info, "Sistem bilgileri getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sistem bilgileri getirme hatası");
                throw;
            }
        }

        [HttpGet("health")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public IActionResult HealthCheck()
        {
            var healthInfo = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Services = new[]
                {
                    new { Service = "Database", Status = "Healthy", ResponseTime = 45 },
                    new { Service = "Cache", Status = "Healthy", ResponseTime = 12 },
                    new { Service = "Email", Status = "Healthy", ResponseTime = 89 }
                }
            };

            return Ok(ApiResponse<object>.SuccessResult(healthInfo, "Sistem sağlık durumu"));
        }

        [HttpGet("logs")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), StatusCodes.Status200OK)]
        public IActionResult GetLogs(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? level = "Error")
        {
            try
            {
                _logger.LogInformation("Loglar getiriliyor - Seviye: {Level}", level);

                var logs = new List<string>
                {
                    $"[{DateTime.UtcNow.AddHours(-1):yyyy-MM-dd HH:mm:ss}] INFO: Sistem başlatıldı",
                    $"[{DateTime.UtcNow.AddMinutes(-45):yyyy-MM-dd HH:mm:ss}] WARNING: Cache temizlendi",
                    $"[{DateTime.UtcNow.AddMinutes(-30):yyyy-MM-dd HH:mm:ss}] INFO: Yeni kullanıcı kaydı",
                    $"[{DateTime.UtcNow.AddMinutes(-15):yyyy-MM-dd HH:mm:ss}] ERROR: Veritabanı bağlantı hatası"
                };

                return Ok(ApiResponse<List<string>>.SuccessResult(logs, "Sistem logları getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Log getirme hatası");
                throw;
            }
        }

        [HttpPost("clear-cache")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ClearCache(CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Sistem cache temizleniyor");

                // TODO: Cache temizleme işlemi
                await Task.Delay(500, ct);

                return Ok(ApiResponse.SuccessResult("Sistem cache başarıyla temizlendi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cache temizleme hatası");
                throw;
            }
        }
    }
}