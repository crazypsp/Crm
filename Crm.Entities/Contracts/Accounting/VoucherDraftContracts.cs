
namespace Crm.Entities.Contracts.Accounting
{
    /// <summary>
    /// Agent'a fiş yazdırmak için navigation property içermeyen DTO.
    /// </summary>
    public sealed record VoucherDraftDto(
        Guid VoucherDraftId,
        DateTime VoucherDate,
        string Description,
        string BankAccountCode,
        IReadOnlyList<VoucherDraftLineDto> Lines
    );

    public sealed record VoucherDraftLineDto(
        int LineNo,
        string AccountCode,
        decimal Debit,
        decimal Credit,
        string? LineDescription,
        string? CostCenterCode
    );

    public sealed record PostVoucherCommand(
        Guid IntegrationProfileId,
        VoucherDraftDto Draft
    );
}
