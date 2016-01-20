using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IArticleRepository
    {
        int Create(ArticleInfo entity);

        bool Update(ArticleInfo entity);
        bool Update(UpdateContext<ArticleInfo> context);

        bool DeleteById(int id);
        bool DeleteById(List<int> ids);

        long Count(QueryDescriptor<ArticleInfo> q);
        PagedList<ArticleInfo> QueryPaged(QueryDescriptor<ArticleInfo> q);
        List<ArticleInfo> Query(QueryDescriptor<ArticleInfo> q);

        ArticleInfo FindById(int id);
    }
}
