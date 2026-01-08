namespace Crm.Services.Integration
{
    public interface IIntegrationDispatcher
    {
        Task<Guid> EnqueuePostVoucherAsync(
            Guid tenantId,
            Guid companyId,
            Guid integrationProfileId,
            Guid voucherDraftId,
            CancellationToken ct);
    }
}