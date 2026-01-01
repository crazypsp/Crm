namespace Crm.Api.Reporting.Contracts
{
    public sealed class StatusCountDto
    {
        public int Status { get; set; }
        public int Count { get; set; }
    }

    public sealed class DashboardSummaryDto
    {
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        // Neden: Üst KPI’lar (dashboard kartları)
        public int CompaniesCount { get; set; }
        public int OpenTasksCount { get; set; }
        public int PendingDocumentsCount { get; set; }
        public int BankImportsCount { get; set; }
        public int BankTransactionsLast30Days { get; set; }
        public int MessagesLast7Days { get; set; }

        // Neden: “Duruma göre dağılım” grafikleri
        public List<StatusCountDto> TaskByStatus { get; set; } = new();
        public List<StatusCountDto> DocumentRequestsByStatus { get; set; } = new();
        public List<StatusCountDto> BankImportsByStatus { get; set; } = new();
    }

    public sealed class CompanyComplianceDto
    {
        public Guid CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int OpenTasks { get; set; }
        public int PendingDocuments { get; set; }

        public DateTimeOffset? LastBankImportAt { get; set; }
        public DateTimeOffset? LastMessageAt { get; set; }
    }
}
