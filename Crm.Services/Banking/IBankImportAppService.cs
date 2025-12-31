
namespace Crm.Services.Banking
{
    public interface IBankImportAppService
    {
        Task<Guid> UploadAndCreateImportAsync(
            Guid tenantId,
            Guid companyId,
            Guid bankAccountId,
            Guid templateId,
            Stream file,
            string fileName,
            string contentType,
            CancellationToken ct);

        Task ApplyMappingAsync(Guid tenantId, Guid importId, CancellationToken ct);

        Task<Guid> BuildDraftAsync(Guid tenantId, Guid importId, string bankAccountCode, CancellationToken ct);
    }
}
