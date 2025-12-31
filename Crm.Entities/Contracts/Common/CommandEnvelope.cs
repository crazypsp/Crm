
namespace Crm.Entities.Contracts.Common
{
    /// <summary>
    /// SignalR üzerinden komut taşımak için standart zarf.
    /// </summary>
    public sealed record CommandEnvelope(
        Guid CommandId,
        Guid TenantId,
        Guid? CompanyId,
        string CommandType,
        string PayloadJson,
        DateTimeOffset CreatedAtUtc
    );
}
