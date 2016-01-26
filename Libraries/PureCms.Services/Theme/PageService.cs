using PureCms.Core.Domain.Theme;
using PureCms.Data;
using PureCms.Core.Context;
using System.Collections.Generic;
using System;

namespace PureCms.Services.Theme
{
    public class PageService
    {
        private DataRepository<PageInfo> _repository = new DataRepository<PageInfo>();

        public int Create(PageInfo entity)
        {
            return _repository.Create(entity);
        }

        public bool DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public bool Update(PageInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<PageInfo>, UpdateContext<PageInfo>> container)
        {
            UpdateContext<PageInfo> context = container(new UpdateContext<PageInfo>());
            return _repository.Update(context);
        }
        public PageInfo FindById(int id)
        {
            return _repository.FindById(id);
        }

        public PagedList<PageInfo> QueryPaged(Func<QueryDescriptor<PageInfo>, QueryDescriptor<PageInfo>> container)
        {
            QueryDescriptor<PageInfo> q = container(new QueryDescriptor<PageInfo>());

            return _repository.QueryPaged(q);
        }
        public List<PageInfo> Query(Func<QueryDescriptor<PageInfo>, QueryDescriptor<PageInfo>> container)
        {
            QueryDescriptor<PageInfo> q = container(new QueryDescriptor<PageInfo>());

            return _repository.Query(q);
        }
    }
}
