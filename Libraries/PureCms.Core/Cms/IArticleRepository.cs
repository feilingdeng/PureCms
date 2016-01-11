using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IArticleRepository
    {
        long Create(ArticleInfo entity);

        bool Update(ArticleInfo entity);
        bool Update(UpdateContext<ArticleInfo> q);

        bool DeleteById(long id);
        bool DeleteById(List<long> ids);

        long Count(ArticleQueryContext q);
        PagedList<ArticleInfo> Query(ArticleQueryContext q);

        ArticleInfo GetById(long id);
    }
}
