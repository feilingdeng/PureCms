using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface IQueryViewRepository
    {
        bool Create(QueryViewInfo entity);

        bool Update(QueryViewInfo entity);
        bool Update(UpdateContext<QueryViewInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<QueryViewInfo> q);
        PagedList<QueryViewInfo> QueryPaged(QueryDescriptor<QueryViewInfo> q);
        List<QueryViewInfo> Query(QueryDescriptor<QueryViewInfo> q);

        QueryViewInfo FindById(Guid id);
    }
}
