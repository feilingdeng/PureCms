using System.Collections.Generic;

namespace PureCms.Core.Components.Grid
{
    public sealed class RowInfo
    {
        public string Name { get; set; }
        public string Id { get; set; }

        private List<CellInfo> _cells;
        public List<CellInfo> Cells
        {
            get
            {
                if (_cells == null)
                {
                    _cells = new List<CellInfo>();
                }
                return _cells;
            }
            set { _cells = value; }
        }

        public void AddCell(CellInfo cell)
        {
            this.Cells.Add(cell);
        }
    }
}
