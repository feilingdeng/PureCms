using PureCms.Core.Domain.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Components.Grid
{
    public sealed class GridBuilder
    {
        private QueryViewInfo _view;
        public GridBuilder(QueryViewInfo view)
        {
            _view = view;
        }

        public string Render()
        {
            StringBuilder result = new StringBuilder();

            return result.ToString();
        }
    }
}
