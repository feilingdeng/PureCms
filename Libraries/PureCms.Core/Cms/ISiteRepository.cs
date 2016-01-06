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

        long Count(SiteQueryContext q);
        PagedList<SiteInfo> Query(SiteQueryContext q);
        List<SiteInfo> GetAll(SiteQueryContext q);

        SiteInfo GetById(int id);
    }
}
