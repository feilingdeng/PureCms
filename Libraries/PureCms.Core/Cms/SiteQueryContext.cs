using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;

namespace PureCms.Core.Cms
{
    public class SiteQueryContext : QueryDescriptor<SiteInfo, SiteQueryContext>
    {
        public string Logo { get; set; }

        public string Name { get; set; }

        public string Theme { get; set; }

        public string Url { get; set; }

        public bool? IsEnabled { get; set; }
        public bool? IsDefault { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
