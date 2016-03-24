using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface ISystemFormRepository
    {
        bool Create(SystemFormInfo entity);

        bool Update(SystemFormInfo entity);
        bool Update(UpdateContext<SystemFormInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<SystemFormInfo> q);
        PagedList<SystemFormInfo> QueryPaged(QueryDescriptor<SystemFormInfo> q);
        List<SystemFormInfo> Query(QueryDescriptor<SystemFormInfo> q);

        SystemFormInfo FindById(Guid id);
    }
}
