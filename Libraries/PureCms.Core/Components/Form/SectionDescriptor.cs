using System.Collections.Generic;

namespace PureCms.Core.Components.Form
{
    public sealed class SectionDescriptor
    {
        public string Name { get; set; }

        public string Label { get; set; }
        public bool IsShowLabel { get; set; }

        public bool IsVisible { get; set; }
        public int Columns { get; set; }

        public List<RowDescriptor> Rows { get; set; }
    }
}
