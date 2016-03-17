using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface IRelationShipRepository
    {
        int Create(RelationShipInfo entity);

        bool Update(RelationShipInfo entity);
        bool Update(UpdateContext<RelationShipInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<RelationShipInfo> q);
        PagedList<RelationShipInfo> QueryPaged(QueryDescriptor<RelationShipInfo> q);
        List<RelationShipInfo> Query(QueryDescriptor<RelationShipInfo> q);
        List<RelationShipInfo> QueryByEntityId(Guid? referencingEntityId, Guid? referencedEntityId);

        RelationShipInfo FindById(Guid id);
    }
}
