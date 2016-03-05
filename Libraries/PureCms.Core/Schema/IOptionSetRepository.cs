using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface IOptionSetRepository
    {
        bool Create(OptionSetInfo entity);
        bool Create(OptionSetInfo entity, List<OptionSetDetailInfo> details);

        bool Update(OptionSetInfo entity);
        bool Update(UpdateContext<OptionSetInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<OptionSetInfo> q);
        PagedList<OptionSetInfo> QueryPaged(QueryDescriptor<OptionSetInfo> q);
        List<OptionSetInfo> Query(QueryDescriptor<OptionSetInfo> q);

        OptionSetInfo FindById(Guid id);
    }
}
