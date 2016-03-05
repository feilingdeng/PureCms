using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class EntityService
    {
        IEntityRepository _repository = new EntityRepository();


        public bool Create(EntityInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(EntityInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<EntityInfo>, UpdateContext<EntityInfo>> context)
        {
            var ctx = context(new UpdateContext<EntityInfo>());
            return _repository.Update(ctx);
        }

        public EntityInfo FindById(Guid id)
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

        public PagedList<EntityInfo> QueryPaged(Func<QueryDescriptor<EntityInfo>, QueryDescriptor<EntityInfo>> container)
        {
            QueryDescriptor<EntityInfo> q = container(new QueryDescriptor<EntityInfo>());

            return _repository.QueryPaged(q);
        }
        public List<EntityInfo> Query(Func<QueryDescriptor<EntityInfo>, QueryDescriptor<EntityInfo>> container)
        {
            QueryDescriptor<EntityInfo> q = container(new QueryDescriptor<EntityInfo>());

            return _repository.Query(q);
        }
    }
}
