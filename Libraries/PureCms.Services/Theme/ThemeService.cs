using PureCms.Core.Context;
using PureCms.Core.Cms;
using PureCms.Data.Cms;
using System;
using System.Collections.Generic;
using PureCms.Core.Domain.Theme;

namespace PureCms.Services.Theme
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
            return _repository.FindById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public PagedList<ThemeInfo> Query(Func<QueryDescriptor<ThemeInfo>, QueryDescriptor<ThemeInfo>> container)
        {
            QueryDescriptor<ThemeInfo> q = container(new QueryDescriptor<ThemeInfo>());

            return _repository.QueryPaged(q);
        }
        public List<ThemeInfo> GetAll(Func<QueryDescriptor<ThemeInfo>, QueryDescriptor<ThemeInfo>> container)
        {
            QueryDescriptor<ThemeInfo> q = container(new QueryDescriptor<ThemeInfo>());

            return _repository.Query(q);
        }
    }
}
