using PureCms.Core.Domain.Theme;
using PureCms.Data;
using PureCms.Core.Context;
using System.Collections.Generic;
using System;

namespace PureCms.Services.Theme
{
    public class ThemeModuleService
    {
        private DataRepository<ThemeModuleInfo> _repository = new DataRepository<ThemeModuleInfo>();

        public int Create(ThemeModuleInfo entity)
        {
            return _repository.Create(entity);
        }

        public bool DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public bool Update(ThemeModuleInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<ThemeModuleInfo>, UpdateContext<ThemeModuleInfo>> container)
        {
            UpdateContext<ThemeModuleInfo> context = container(new UpdateContext<ThemeModuleInfo>());
            return _repository.Update(context);
        }
        public ThemeModuleInfo FindById(int id)
        {
            return _repository.FindById(id);
        }

        public PagedList<ThemeModuleInfo> QueryPaged(Func<QueryDescriptor<ThemeModuleInfo>, QueryDescriptor<ThemeModuleInfo>> container)
        {
            QueryDescriptor<ThemeModuleInfo> q = container(new QueryDescriptor<ThemeModuleInfo>());

            return _repository.QueryPaged(q);
        }
        public List<ThemeModuleInfo> Query(Func<QueryDescriptor<ThemeModuleInfo>, QueryDescriptor<ThemeModuleInfo>> container)
        {
            QueryDescriptor<ThemeModuleInfo> q = container(new QueryDescriptor<ThemeModuleInfo>());

            return _repository.Query(q);
        }
    }
}
