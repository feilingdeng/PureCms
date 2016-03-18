using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Components.Form
{
    public sealed class NavGroupDescriptor
    {
        public string Label { get; set; }
        public bool IsVisible { get; set; }

        public List<NavDescriptor> NavItems { get; set; }
    }
}
