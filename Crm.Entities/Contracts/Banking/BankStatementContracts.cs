using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Entities.Contracts.Banking
{
    /// <summary>
    /// Documents API'nin satır çıkarma sonucu: kolon adı -> değer.
    /// </summary>
    public sealed record RawRow(
        int RowNo,
        IReadOnlyDictionary<string, string?> Cells
    );

    public sealed record ExtractResult(
        IReadOnlyList<RawRow> Rows,
        string? DetectedSheetOrPage,
        string? Notes
    );
}
