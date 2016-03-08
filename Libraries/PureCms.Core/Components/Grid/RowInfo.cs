using System.Collections.Generic;

namespace PureCms.Core.Components.Grid
{
    public sealed class RowInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public List<CellInfo> Cells { get; set; }
    }
}
