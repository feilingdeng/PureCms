using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Components.Form
{
    public sealed class RowDescriptor
    {
        public bool IsVisible { get; set; }
        public List<CellDescriptor> Cells { get; set; }
    }
}
