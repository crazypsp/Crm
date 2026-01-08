namespace Crm.Entities.Contracts.Banking
{
    public sealed class ExtractResult
    {
        public IReadOnlyList<RawRow> Rows { get; }
        public string SourceName { get; }
        public string? Message { get; }

        public ExtractResult(IReadOnlyList<RawRow> rows, string sourceName, string? message = null)
        {
            Rows = rows;
            SourceName = sourceName;
            Message = message;
        }
    }
}