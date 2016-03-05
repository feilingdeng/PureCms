using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

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

        EntityInfo FindById(Guid id);
    }
}
