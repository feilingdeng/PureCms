using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Core.Schema
{
    public interface IOptionSetDetailRepository
    {
        bool Create(OptionSetDetailInfo entity);

        bool Update(OptionSetDetailInfo entity);
        bool Update(UpdateContext<OptionSetDetailInfo> context);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        long Count(QueryDescriptor<OptionSetDetailInfo> q);
        PagedList<OptionSetDetailInfo> QueryPaged(QueryDescriptor<OptionSetDetailInfo> q);
        List<OptionSetDetailInfo> Query(QueryDescriptor<OptionSetDetailInfo> q);

        OptionSetDetailInfo FindById(Guid id);
    }
}
