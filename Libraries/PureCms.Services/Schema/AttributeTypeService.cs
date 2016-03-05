using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class AttributeTypeService
    {
        IAttributeTypeRepository _repository = new AttributeTypeRepository();
        
        public AttributeTypeInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public AttributeTypeInfo FindByName(string name)
        {
            return _repository.FindByName(name);
        }

        public PagedList<AttributeTypeInfo> Query(Func<QueryDescriptor<AttributeTypeInfo>, QueryDescriptor<AttributeTypeInfo>> container)
        {
            QueryDescriptor<AttributeTypeInfo> q = container(new QueryDescriptor<AttributeTypeInfo>());

            return _repository.QueryPaged(q);
        }
        public List<AttributeTypeInfo> GetAll(Func<QueryDescriptor<AttributeTypeInfo>, QueryDescriptor<AttributeTypeInfo>> container)
        {
            QueryDescriptor<AttributeTypeInfo> q = container(new QueryDescriptor<AttributeTypeInfo>());

            return _repository.Query(q);
        }
    }
}
