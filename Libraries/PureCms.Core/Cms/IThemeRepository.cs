using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IThemeRepository
    {
        int Create(ThemeInfo entity);

        bool Update(ThemeInfo entity);

        bool DeleteById(int id);

        long Count(QueryDescriptor<ThemeInfo> q);
        PagedList<ThemeInfo> Query(QueryDescriptor<ThemeInfo> q);
        List<ThemeInfo> GetAll(QueryDescriptor<ThemeInfo> q);

        ThemeInfo GetById(int id);
    }
}
