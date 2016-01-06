using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System;

namespace PureCms.Core.Cms
{
    public class ChannelQueryContext : QueryDescriptor<ChannelInfo, ChannelQueryContext>
    {
        public int? ChannelId { get; set; }
        public int? SiteId { get; set; }
        public int? ParentChannelId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string OpenTarget { get; set; }
        public int? Level { get; set; }
        public ContentType? ContentType { get; set; }
        public bool? IsShow { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsEnabled { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
