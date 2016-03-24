using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;


namespace PureCms.Services.Schema
{
    public class SystemFormService
    {
        ISystemFormRepository _repository = new SystemFormRepository();


        public bool Create(SystemFormInfo systemForm)
        {
            return _repository.Create(systemForm);
        }
        public bool Update(SystemFormInfo systemForm)
        {
            return _repository.Update(systemForm);
        }
        public bool Update(Func<UpdateContext<SystemFormInfo>, UpdateContext<SystemFormInfo>> context)
        {
            var ctx = context(new UpdateContext<SystemFormInfo>());
            return _repository.Update(ctx);
        }

        public SystemFormInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<Guid> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<SystemFormInfo> QueryPaged(Func<QueryDescriptor<SystemFormInfo>, QueryDescriptor<SystemFormInfo>> container)
        {
            QueryDescriptor<SystemFormInfo> q = container(new QueryDescriptor<SystemFormInfo>());

            return _repository.QueryPaged(q);
        }

        public List<SystemFormInfo> Query(Func<QueryDescriptor<SystemFormInfo>, QueryDescriptor<SystemFormInfo>> container)
        {
            QueryDescriptor<SystemFormInfo> q = container(new QueryDescriptor<SystemFormInfo>());

            return _repository.Query(q);
        }
    }
}
