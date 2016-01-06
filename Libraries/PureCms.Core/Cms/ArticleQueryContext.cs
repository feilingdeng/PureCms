using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public class ArticleQueryContext : QueryDescriptor<ArticleInfo, ArticleQueryContext>
    {
        public List<long> ArticleIdList { get; set; }
        public long? ArticleId { get; set; }
        public int? CategoryId { get; set; }
        public int? ChannelId { get; set; }
        public int? Status { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Author { get; set; }
        public string Url { get; set; }

        public string Content { get; set; }

        public bool? IsShow { get; set; }
        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
