
namespace Crm.Entities.Enums
{
    public enum BankImportStatus
    {
        Uploaded = 1,
        Extracted = 2,
        Normalized = 3,
        Mapped = 4,
        DraftCreated = 5,
        DispatchedToAgent = 6,
        Completed = 7,
        Failed = 99
    }
}
