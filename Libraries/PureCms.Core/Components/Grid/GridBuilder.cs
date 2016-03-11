using PureCms.Core.Domain.Query;
using System.Text;

namespace PureCms.Core.Components.Grid
{
    public sealed class GridBuilder
    {
        private QueryViewInfo _view;
        private GridInfo _grid;
        public GridBuilder(QueryViewInfo view)
        {
            _view = view;
            _grid = _grid.DeserializeFromJson(_view.LayoutConfig);
        }

        public GridInfo Grid
        {
            get
            {
                return _grid;
            }
            set { _grid = value; }
        }
        public string Render()
        {
            StringBuilder result = new StringBuilder();
            //_grid = _grid.DeserializeFromJson(_view.LayoutConfig);

            return result.ToString();
        }
    }
}
