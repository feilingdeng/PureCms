using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface ISiteRepository
    {
        int Create(SiteInfo entity);

        bool Update(SiteInfo entity);

        bool DeleteById(int id);

        long Count(QueryDescriptor<SiteInfo> q);
        PagedList<SiteInfo> Query(QueryDescriptor<SiteInfo> q);
        List<SiteInfo> GetAll(QueryDescriptor<SiteInfo> q);

        SiteInfo GetById(int id);
    }
}
