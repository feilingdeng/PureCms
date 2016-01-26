using PureCms.Core.Domain.Theme;
using PureCms.Data;
using PureCms.Core.Context;
using System.Collections.Generic;
using System;

namespace PureCms.Services.Theme
{
    public class PageModuleService
    {
        private DataRepository<PageModuleInfo> _repository = new DataRepository<PageModuleInfo>();

        public bool Create(PageModuleInfo entity)
        {
            return _repository.CreateBool(entity);
        }

        public bool DeleteById(Guid id)
        {
            return _repository.Delete(id);
        }

        public bool Update(PageModuleInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<PageModuleInfo>, UpdateContext<PageModuleInfo>> container)
        {
            UpdateContext<PageModuleInfo> context = container(new UpdateContext<PageModuleInfo>());
            return _repository.Update(context);
        }
        public PageModuleInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public PagedList<PageModuleInfo> QueryPaged(Func<QueryDescriptor<PageModuleInfo>, QueryDescriptor<PageModuleInfo>> container)
        {
            QueryDescriptor<PageModuleInfo> q = container(new QueryDescriptor<PageModuleInfo>());

            return _repository.QueryPaged(q);
        }
        public List<PageModuleInfo> Query(Func<QueryDescriptor<PageModuleInfo>, QueryDescriptor<PageModuleInfo>> container)
        {
            QueryDescriptor<PageModuleInfo> q = container(new QueryDescriptor<PageModuleInfo>());

            return _repository.Query(q);
        }
    }
}
