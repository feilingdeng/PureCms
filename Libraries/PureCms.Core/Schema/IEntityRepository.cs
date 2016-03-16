using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PureCms.Core.Schema
{
    public interface IEntityRepository
    {
        bool Create(EntityInfo entity);

        bool Update(EntityInfo entity);
        bool Update(UpdateContext<EntityInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<EntityInfo> q);
        PagedList<EntityInfo> QueryPaged(QueryDescriptor<EntityInfo> q);
        List<EntityInfo> Query(QueryDescriptor<EntityInfo> q);
        List<EntityInfo> QueryRelated(Guid entityid);

        EntityInfo FindById(Guid id);
        EntityInfo Find(Expression<Func<EntityInfo, bool>> predicate);
    }
}
