namespace Crm.Api.Banking.Contracts
{
    public sealed class UpsertBankTemplateRequest
    {
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }

        // Neden: UI’da template listesi için temel tanım.
        public string Name { get; set; } = default!;

        // Neden: Aynı müşavir farklı bankalar için farklı template tanımlar.
        public string? BankName { get; set; }

        // Neden: PDF/Excel ve kolon haritaları bankadan bankaya değişir.
        // Bu sözlük JSON olarak saklanır (entity’de uygun bir alan varsa).
        public Dictionary<string, string> Definition { get; set; } = new();

        public bool IsActive { get; set; } = true;
    }

    public sealed class BankTemplateDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Name { get; set; }
        public string? BankName { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public sealed class UpsertBankMappingRuleRequest
    {
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? TemplateId { get; set; }

        // Neden: Açıklama içinde yakalanacak anahtar/regex ile otomatik muhasebe hesabı öner.
        public string MatchText { get; set; } = default!;

        // Neden: Karşı hesap kodu (ör: 320, 740, 770 vs.).
        public string CounterAccountCode { get; set; } = default!;

        public decimal? Confidence { get; set; }
        public int? Priority { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public sealed class BankMappingRuleDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? TemplateId { get; set; }
        public string? MatchText { get; set; }
        public string? CounterAccountCode { get; set; }
        public decimal? Confidence { get; set; }
        public int? Priority { get; set; }
        public bool? IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
