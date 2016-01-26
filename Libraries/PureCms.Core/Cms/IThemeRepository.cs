using PureCms.Core.Context;
using PureCms.Core.Domain.Theme;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IThemeRepository
    {
        int Create(ThemeInfo entity);

        bool Update(ThemeInfo entity);
        bool Update(UpdateContext<ThemeInfo> context);

        bool DeleteById(int id);

        long Count(QueryDescriptor<ThemeInfo> q);
        PagedList<ThemeInfo> QueryPaged(QueryDescriptor<ThemeInfo> q);
        List<ThemeInfo> Query(QueryDescriptor<ThemeInfo> q);

        ThemeInfo FindById(int id);
    }
}
