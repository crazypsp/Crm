using Crm.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Banking.AI
{
    public class BankTransactionTrainingData : TenantEntity
    {
        public Guid OriginalTransactionId { get; set; }
        public BankTransaction OriginalTransaction { get; set; } = default!;
        [Required, MaxLength(EntityConstants.MediumTextMax)]
        public string DescriptionKeywordsJson { get; set; } = default!;
        public decimal Amount { get; set; }
        public bool IsOutflow { get; set; }
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        [Required, MaxLength(EntityConstants.AccountCodeMax)]
        public string CorrectAccountCode { get; set; } = default!;
        [MaxLength(EntityConstants.NameMax)]
        public string? CorrectAccountName { get; set; }
        public Guid? AppliedModelVersion { get; set; }
        public decimal? ModelConfidence { get; set; }
        public bool UsedInTraining { get; set; } = false;
        public DateTime? LastTrainingDate { get; set; }
        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Notes { get; set; }
    }
}
