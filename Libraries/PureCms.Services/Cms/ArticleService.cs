using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using PureCms.Core.Utilities;
using PureCms.Data.Cms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PureCms.Services.Cms
{
    public class ArticleService
    {
        IArticleRepository _repository = new ArticleRepository();


        public long Create(ArticleInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(ArticleInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<ArticleInfo, ArticleQueryContext>, UpdateContext<ArticleInfo, ArticleQueryContext>> context)
        {
            var ctx = context(new UpdateContext<ArticleInfo, ArticleQueryContext>());
            List<KeyValuePair<string, object>> sets = ctx.Sets;
            ArticleQueryContext q = ctx.QueryContext;
            return _repository.Update(sets, q);
        }

        public ArticleInfo GetById(long id)
        {
            return _repository.GetById(id);
        }
        public bool DeleteById(long id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<long> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<ArticleInfo> Query(Func<ArticleQueryContext, ArticleQueryContext> container)
        {
            ArticleQueryContext q = container(new ArticleQueryContext());

            return _repository.Query(q);
        }
    }

}
