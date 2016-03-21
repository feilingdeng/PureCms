using System.Collections.Generic;

namespace PureCms.Core.Components.Form
{
    public sealed class PanelDescriptor
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public bool IsExpanded { get; set; }
        public bool IsShowLabel { get; set; }

        public bool IsVisible { get; set; }

        public List<SectionDescriptor> Sections { get; set; }
    }
}
