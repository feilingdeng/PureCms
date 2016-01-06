using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using PureCms.Core.Utilities;
using PureCms.Data.Cms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PureCms.Services.Cms
{
    public class ThemeService
    {
        IThemeRepository _repository = new ThemeRepository();


        public long Create(ThemeInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(ThemeInfo entity)
        {
            return _repository.Update(entity);
        }

        public ThemeInfo GetById(int id)
        {
            return _repository.GetById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public PagedList<ThemeInfo> Query(Func<ThemeQueryContext, ThemeQueryContext> container)
        {
            ThemeQueryContext q = container(new ThemeQueryContext());

            return _repository.Query(q);
        }
        public List<ThemeInfo> GetAll(Func<ThemeQueryContext, ThemeQueryContext> container)
        {
            ThemeQueryContext q = container(new ThemeQueryContext());

            return _repository.GetAll(q);
        }
    }
}
