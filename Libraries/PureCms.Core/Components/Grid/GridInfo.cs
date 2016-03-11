using PureCms.Core.Components.Platform;
using PureCms.Core.Context;
using PureCms.Core.Data;
using System.Collections.Generic;

namespace PureCms.Core.Components.Grid
{
    public sealed class GridInfo
    {
        public List<QueryColumnSortInfo> SortColumns { get; set; }
        public List<RowInfo> Rows { get; set; }

        //public PageList<dynamic> DataSource { get; set; }
    }
}
