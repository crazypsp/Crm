
namespace Crm.Entities.Contracts.Common
{
    public sealed record ResultEnvelope(
    Guid CommandId,
    bool Success,
    string? ResultJson,
    string? ErrorMessage,
    DateTimeOffset CompletedAtUtc
);
}
