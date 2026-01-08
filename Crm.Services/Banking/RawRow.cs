namespace Crm.Entities.Contracts.Banking
{
    public sealed class RawRow
    {
        public int RowNo { get; }
        public IReadOnlyDictionary<string, string?> Cells { get; }

        public RawRow(int rowNo, IReadOnlyDictionary<string, string?> cells)
        {
            RowNo = rowNo;
            Cells = cells;
        }
    }
}