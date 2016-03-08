using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class OptionSetService
    {
        IOptionSetRepository _repository = new OptionSetRepository();


        public bool Create(OptionSetInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Create(OptionSetInfo entity, List<OptionSetDetailInfo> details)
        {
            return _repository.Create(entity, details);
        }
        public bool Update(OptionSetInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<OptionSetInfo>, UpdateContext<OptionSetInfo>> context)
        {
            var ctx = context(new UpdateContext<OptionSetInfo>());
            return _repository.Update(ctx);
        }

        public OptionSetInfo FindById(Guid id)
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

        public PagedList<OptionSetInfo> QueryPaged(Func<QueryDescriptor<OptionSetInfo>, QueryDescriptor<OptionSetInfo>> container)
        {
            QueryDescriptor<OptionSetInfo> q = container(new QueryDescriptor<OptionSetInfo>());

            return _repository.QueryPaged(q);
        }
        public List<OptionSetInfo> Query(Func<QueryDescriptor<OptionSetInfo>, QueryDescriptor<OptionSetInfo>> container)
        {
            QueryDescriptor<OptionSetInfo> q = container(new QueryDescriptor<OptionSetInfo>());

            return _repository.Query(q);
        }
    }
}
