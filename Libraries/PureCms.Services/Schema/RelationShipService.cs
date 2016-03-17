using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class RelationShipService
    {
        IRelationShipRepository _repository = new RelationShipRepository();


        public int Create(RelationShipInfo RelationShip)
        {
            return _repository.Create(RelationShip);
        }
        public bool Update(RelationShipInfo RelationShip)
        {
            return _repository.Update(RelationShip);
        }
        public bool Update(Func<UpdateContext<RelationShipInfo>, UpdateContext<RelationShipInfo>> context)
        {
            var ctx = context(new UpdateContext<RelationShipInfo>());
            return _repository.Update(ctx);
        }

        public RelationShipInfo GetById(Guid id)
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

        public PagedList<RelationShipInfo> QueryPaged(Func<QueryDescriptor<RelationShipInfo>, QueryDescriptor<RelationShipInfo>> container)
        {
            QueryDescriptor<RelationShipInfo> q = container(new QueryDescriptor<RelationShipInfo>());

            return _repository.QueryPaged(q);
        }

        public List<RelationShipInfo> Query(Func<QueryDescriptor<RelationShipInfo>, QueryDescriptor<RelationShipInfo>> container)
        {
            QueryDescriptor<RelationShipInfo> q = container(new QueryDescriptor<RelationShipInfo>());

            return _repository.Query(q);
        }
        public List<RelationShipInfo> QueryByEntityId(Guid? referencingEntityId, Guid? referencedEntityId)
        {
            return _repository.QueryByEntityId(referencingEntityId, referencedEntityId);
        }
    }
}
