
namespace Crm.Entities.Contracts.Integration
{
    public static class IntegrationCommandTypes
    {
        public const string TestConnection = "TestConnection";
        public const string PullChartOfAccounts = "PullChartOfAccounts";
        public const string PostVoucher = "PostVoucher";
        public const string GetBranches = "GetBranches";
    }

    public sealed record TestConnectionCommand(
        Guid IntegrationProfileId
    );

    public sealed record PullChartOfAccountsCommand(
        Guid IntegrationProfileId,
        bool IncludePassive = true
    );

    public sealed record GetBranchesCommand(
        Guid IntegrationProfileId
    );
}
