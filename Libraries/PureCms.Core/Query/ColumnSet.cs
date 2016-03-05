using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PureCms.Core.Query
{
    public sealed class ColumnSet
    {
        private bool _allColumns;
        private List<string> _columns;
        [DataMember]
        public bool AllColumns
        {
            get
            {
                return this._allColumns;
            }
            set
            {
                this._allColumns = value;
            }
        }
        [DataMember]
        public List<string> Columns
        {
            get
            {
                if (this._columns == null)
                {
                    this._columns = new List<string>();
                }
                return this._columns;
            }
        }
        public ColumnSet()
        {
        }
        public ColumnSet(bool allColumns)
        {
            this._allColumns = allColumns;
        }
        public ColumnSet(params string[] columns)
        {
            this._columns = new List<string>(columns);
        }
        public void AddColumns(params string[] columns)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                string item = columns[i];
                this.Columns.Add(item);
            }
        }
        public void AddColumn(string column)
        {
            this.Columns.Add(column);
        }
    }
}
