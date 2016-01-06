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

        long Count(ThemeQueryContext q);
        PagedList<ThemeInfo> Query(ThemeQueryContext q);
        List<ThemeInfo> GetAll(ThemeQueryContext q);

        ThemeInfo GetById(int id);
    }
}
