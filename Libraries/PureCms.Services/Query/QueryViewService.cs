using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Query
{
    public class QueryViewService
    {
        IQueryViewRepository _repository = new QueryViewRepository();


        public bool Create(QueryViewInfo QueryView)
        {
            return _repository.Create(QueryView);
        }
        public bool Update(QueryViewInfo QueryView)
        {
            return _repository.Update(QueryView);
        }
        public bool Update(Func<UpdateContext<QueryViewInfo>, UpdateContext<QueryViewInfo>> context)
        {
            var ctx = context(new UpdateContext<QueryViewInfo>());
            return _repository.Update(ctx);
        }

        public QueryViewInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<Guid> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<QueryViewInfo> Query(Func<QueryDescriptor<QueryViewInfo>, QueryDescriptor<QueryViewInfo>> container)
        {
            QueryDescriptor<QueryViewInfo> q = container(new QueryDescriptor<QueryViewInfo>());

            return _repository.QueryPaged(q);
        }
    }
}
