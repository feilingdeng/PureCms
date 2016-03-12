using PureCms.Core.Domain.Theme;
using PureCms.Data;
using PureCms.Core.Context;
using System.Collections.Generic;
using System;

namespace PureCms.Services.Theme
{
    public class PageLayoutService
    {
        private DataRepository<PageLayoutInfo> _repository = new DataRepository<PageLayoutInfo>();

        //public Func<PageLayoutInfo, bool> Create;

        public PageLayoutService()
        {
            //Create = _repository.CreateBool;
        }
        public bool Create(PageLayoutInfo entity)
        {
            return _repository.CreateObject(entity);
        }

        public bool DeleteById(Guid id)
        {
            return _repository.Delete(id);
        }

        public bool Update(PageLayoutInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<PageLayoutInfo>, UpdateContext<PageLayoutInfo>> container)
        {
            UpdateContext<PageLayoutInfo> context = container(new UpdateContext<PageLayoutInfo>());
            return _repository.Update(context);
        }
        public PageLayoutInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public PagedList<PageLayoutInfo> QueryPaged(Func<QueryDescriptor<PageLayoutInfo>, QueryDescriptor<PageLayoutInfo>> container)
        {
            QueryDescriptor<PageLayoutInfo> q = container(new QueryDescriptor<PageLayoutInfo>());

            return _repository.QueryPaged(q);
        }
        public List<PageLayoutInfo> Query(Func<QueryDescriptor<PageLayoutInfo>, QueryDescriptor<PageLayoutInfo>> container)
        {
            QueryDescriptor<PageLayoutInfo> q = container(new QueryDescriptor<PageLayoutInfo>());

            return _repository.Query(q);
        }
    }
}
