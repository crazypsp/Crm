using System.ComponentModel.DataAnnotations;
using Crm.Entities.Common;
using Crm.Entities.Documents;
using Crm.Entities.Enums;
using Crm.Entities.Tenancy;

namespace Crm.Entities.Banking
{
    /// <summary>
    /// Bir dosya yükleme/import oturumu.
    /// </summary>
    public class BankStatementImport : TenantEntity
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public Guid BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; } = default!;

        public Guid TemplateId { get; set; }
        public BankTemplate Template { get; set; } = default!;

        public Guid SourceFileId { get; set; }
        public DocumentFile SourceFile { get; set; } = default!;

        public BankImportStatus Status { get; set; } = BankImportStatus.Uploaded;

        public int TotalRows { get; set; }
        public int ImportedRows { get; set; }

        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Notes { get; set; }

        public ICollection<BankTransaction> Transactions { get; set; } = new List<BankTransaction>();
    }
}
