using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class AttributeService
    {
        IAttributeRepository _repository = new AttributeRepository();


        public bool Create(AttributeInfo Attribute)
        {
            return _repository.Create(Attribute);
        }
        public bool Update(AttributeInfo Attribute)
        {
            return _repository.Update(Attribute);
        }
        public bool Update(Func<UpdateContext<AttributeInfo>, UpdateContext<AttributeInfo>> context)
        {
            var ctx = context(new UpdateContext<AttributeInfo>());
            return _repository.Update(ctx);
        }

        public AttributeInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        public AttributeInfo Find(Guid entityId, string name)
        {
            return _repository.Find(entityId, name);
        }
        public bool DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<Guid> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<AttributeInfo> QueryPaged(Func<QueryDescriptor<AttributeInfo>, QueryDescriptor<AttributeInfo>> container)
        {
            QueryDescriptor<AttributeInfo> q = container(new QueryDescriptor<AttributeInfo>());

            return _repository.QueryPaged(q);
        }
        public List<AttributeInfo> GetAll(Func<QueryDescriptor<AttributeInfo>, QueryDescriptor<AttributeInfo>> container)
        {
            QueryDescriptor<AttributeInfo> q = container(new QueryDescriptor<AttributeInfo>());

            return _repository.Query(q);
        }
    }
}
