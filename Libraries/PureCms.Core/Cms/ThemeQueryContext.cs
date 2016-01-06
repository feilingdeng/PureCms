using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;

namespace PureCms.Core.Cms
{
    public class ThemeQueryContext : QueryDescriptor<ThemeInfo, ThemeQueryContext>
    {

        public string PathName { get; set; }

        public string DisplayName { get; set; }

        public string Author { get; set; }
        public string Picture { get; set; }

        public bool? IsEnabled { get; set; }
        public string Version { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
