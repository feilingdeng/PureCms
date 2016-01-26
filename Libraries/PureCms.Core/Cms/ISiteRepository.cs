using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface ISiteRepository
    {
        int Create(SiteInfo entity);

        bool Update(SiteInfo entity);
        bool Update(UpdateContext<SiteInfo> context);

        bool DeleteById(int id);

        long Count(QueryDescriptor<SiteInfo> q);
        PagedList<SiteInfo> QueryPaged(QueryDescriptor<SiteInfo> q);
        List<SiteInfo> Query(QueryDescriptor<SiteInfo> q);

        SiteInfo FindById(int id);
    }
}
