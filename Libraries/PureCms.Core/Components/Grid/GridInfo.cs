using PureCms.Core.Components.Platform;
using PureCms.Core.Context;
using PureCms.Core.Data;
using System.Collections.Generic;

namespace PureCms.Core.Components.Grid
{
    public sealed class GridInfo
    {
        private List<QueryColumnSortInfo> _sortColumns;
        public List<QueryColumnSortInfo> SortColumns {
            get
            {
                if(_sortColumns == null)
                {
                    _sortColumns = new List<QueryColumnSortInfo>();
                }
                return _sortColumns;
            }
            set { _sortColumns = value; }
        }
        private List<RowInfo> _rows;
        public List<RowInfo> Rows
        {
            get
            {
                if (_rows == null)
                {
                    _rows = new List<RowInfo>();
                }
                return _rows;
            }
            set { _rows = value; }
        }

        //public PageList<dynamic> DataSource { get; set; }

        public void AddRow(RowInfo row)
        {
            this.Rows.Add(row);
        }
        public void AddSort(QueryColumnSortInfo sort)
        {
            this.SortColumns.Add(sort);
        }
    }
}
