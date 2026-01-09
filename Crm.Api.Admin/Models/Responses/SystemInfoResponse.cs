namespace Crm.Api.Admin.Models.Responses
{
    public class SystemInfoResponse
    {
        public string Version { get; set; } = "1.0.0";
        public DateTime ServerTime { get; set; } = DateTime.UtcNow;
        public DatabaseInfo Database { get; set; } = new();
        public SystemStats Stats { get; set; } = new();
    }

    public class DatabaseInfo
    {
        public string Provider { get; set; } = "SQL Server";
        public string Status { get; set; } = "Healthy";
        public DateTime LastBackup { get; set; } = DateTime.UtcNow.AddDays(-1);
    }

    public class SystemStats
    {
        public int TotalTenants { get; set; }
        public int TotalCompanies { get; set; }
        public int TotalUsers { get; set; }
        public int ActiveSessions { get; set; }
        public decimal MemoryUsageMB { get; set; }
        public decimal CpuUsagePercent { get; set; }
    }
}