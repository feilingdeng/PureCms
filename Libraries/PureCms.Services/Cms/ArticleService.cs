using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using PureCms.Data.Cms;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Cms
{
    public class ArticleService
    {
        IArticleRepository _repository = new ArticleRepository();


        public int Create(ArticleInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(ArticleInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<ArticleInfo>, UpdateContext<ArticleInfo>> context)
        {
            var ctx = context(new UpdateContext<ArticleInfo>());
            return _repository.Update(ctx);
        }

        public ArticleInfo FindById(int id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<int> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<ArticleInfo> QueryPaged(Func<QueryDescriptor<ArticleInfo>, QueryDescriptor<ArticleInfo>> container)
        {
            QueryDescriptor<ArticleInfo> q = container(new QueryDescriptor<ArticleInfo>());

            return _repository.QueryPaged(q);
        }
    }

}
