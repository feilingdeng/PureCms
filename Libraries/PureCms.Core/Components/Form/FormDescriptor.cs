using System.Collections.Generic;

namespace PureCms.Core.Components.Form
{
    public sealed class FormDescriptor
    {
        public const FormType DefaultFormType = FormType.Main;

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsShowNav { get; set; }

        public PanelDescriptor Header { get; set; }
        public PanelDescriptor Footer { get; set; }

        public List<NavGroupDescriptor> NavGroups { get; set; }
        public List<PanelDescriptor> Panels { get; set; }
    }
}
