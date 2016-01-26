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
    public class SiteService
    {
        ISiteRepository _repository = new SiteRepository();


        public long Create(SiteInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(SiteInfo entity)
        {
            return _repository.Update(entity);
        }

        public SiteInfo GetById(int id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public PagedList<SiteInfo> Query(Func<QueryDescriptor<SiteInfo>, QueryDescriptor<SiteInfo>> container)
        {
            QueryDescriptor<SiteInfo> q = container(new QueryDescriptor<SiteInfo>());

            return _repository.QueryPaged(q);
        }
    }
}
