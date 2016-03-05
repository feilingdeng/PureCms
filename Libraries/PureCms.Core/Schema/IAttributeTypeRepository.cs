using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface IAttributeTypeRepository
    {

        long Count(QueryDescriptor<AttributeTypeInfo> q);
        PagedList<AttributeTypeInfo> QueryPaged(QueryDescriptor<AttributeTypeInfo> q);
        List<AttributeTypeInfo> Query(QueryDescriptor<AttributeTypeInfo> q);

        AttributeTypeInfo FindById(Guid id);
        AttributeTypeInfo FindByName(string name);
    }
}
