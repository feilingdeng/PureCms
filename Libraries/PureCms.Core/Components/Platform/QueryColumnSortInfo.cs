namespace PureCms.Core.Components.Platform
{
    public sealed class QueryColumnSortInfo
    {
        private string _name = string.Empty;
        private bool _sortAscending = true;
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
        public bool SortAscending
        {
            get
            {
                return this._sortAscending;
            }
            set
            {
                this._sortAscending = value;
            }
        }
        public string ParameterValue
        {
            get
            {
                return this.Name + ":" + (this.SortAscending ? "1" : "0");
            }
        }
        public QueryColumnSortInfo(string name, bool sortAscending)
        {
            this.Name = name;
            this.SortAscending = sortAscending;
        }
        public QueryColumnSortInfo()
        {
        }
    }
}
