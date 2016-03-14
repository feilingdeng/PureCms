using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PureCms.Core.Schema
{
    public interface IAttributeRepository
    {
        bool Create(AttributeInfo entity);
        bool CreateMany(List<AttributeInfo> entities);
        bool Update(AttributeInfo entity);
        bool Update(UpdateContext<AttributeInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<AttributeInfo> q);
        PagedList<AttributeInfo> QueryPaged(QueryDescriptor<AttributeInfo> q);
        List<AttributeInfo> Query(QueryDescriptor<AttributeInfo> q);

        AttributeInfo FindById(Guid id);
        AttributeInfo Find(Guid entityId, string name);
        AttributeInfo Find(Expression<Func<AttributeInfo, bool>> predicate);
    }
}
