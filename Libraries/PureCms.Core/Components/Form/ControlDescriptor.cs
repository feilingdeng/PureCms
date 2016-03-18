using PureCms.Core.Domain.Schema;

namespace PureCms.Core.Components.Form
{
    public sealed class ControlDescriptor
    {
        public string Name { get; set; }
        public string EntityName { get; set; }

        public AttributeInfo AttributeMetadata { get; set; }
    }
}
