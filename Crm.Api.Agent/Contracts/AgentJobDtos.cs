namespace Crm.Api.Agent.Contracts
{
    
    /// <summary>
    /// Neden: Agent “bir sonraki işi” çekerken, API sadece çalıştırması gereken minimum alanı döner.
    /// Payload içinde işin ne yapacağı (PostVoucherDraft vb) bulunur.
    /// </summary>
    public sealed class NextJobResponse
    {
        public Guid JobId { get; set; }
        public string JobType { get; set; } = default!;
        public string PayloadJson { get; set; } = default!;
        public DateTimeOffset LockedUntil { get; set; }
        public int Attempts { get; set; }
    }

    /// <summary>
    /// Neden: Agent işi başarıyla bitirdiğinde job status’u Completed yapılır.
    /// ERP’de oluşan fiş numarası vb bilgi geri yazılabilir.
    /// </summary>
    public sealed class CompleteJobRequest
    {
        public string? ResultJson { get; set; }
    }

    /// <summary>
    /// Neden: Hata durumunda agent hata mesajını server’a taşır. Retry/dead-letter kararını server verir.
    /// </summary>
    public sealed class FailJobRequest
    {
        public string ErrorMessage { get; set; } = default!;
    }

}
