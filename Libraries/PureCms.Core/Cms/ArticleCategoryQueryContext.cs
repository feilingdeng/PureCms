using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;

namespace PureCms.Core.Cms
{
    public class ArticleCategoryQueryContext : QueryDescriptor<ArticleCategoryInfo, ArticleCategoryQueryContext>
    {
        public int? ArticleCategoryId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public int? ParentArticleCategoryId { get; set; }

        public int? Level { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsEnable { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
