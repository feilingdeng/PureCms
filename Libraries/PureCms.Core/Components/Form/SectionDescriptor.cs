using System.Collections.Generic;

namespace PureCms.Core.Components.Form
{
    public sealed class SectionDescriptor
    {
        public string Name { get; set; }

        public string Label { get; set; }
        public bool IsShowLabel { get; set; }

        public bool IsVisible { get; set; }

        private int _columns = 2;
        public int Columns
        {
            get
            {
                return this._columns;
            }
        }

        public List<RowDescriptor> Rows { get; set; }
    }
}
