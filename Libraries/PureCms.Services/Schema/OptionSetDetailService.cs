using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Schema
{
    public class OptionSetDetailDetailService
    {
        IOptionSetDetailRepository _repository = new OptionSetDetailRepository();


        public bool Create(OptionSetDetailInfo OptionSetDetail)
        {
            return _repository.Create(OptionSetDetail);
        }
        public bool Update(OptionSetDetailInfo OptionSetDetail)
        {
            return _repository.Update(OptionSetDetail);
        }
        public bool Update(Func<UpdateContext<OptionSetDetailInfo>, UpdateContext<OptionSetDetailInfo>> context)
        {
            var ctx = context(new UpdateContext<OptionSetDetailInfo>());
            return _repository.Update(ctx);
        }

        public OptionSetDetailInfo GetById(Guid id)
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

        public PagedList<OptionSetDetailInfo> Query(Func<QueryDescriptor<OptionSetDetailInfo>, QueryDescriptor<OptionSetDetailInfo>> container)
        {
            QueryDescriptor<OptionSetDetailInfo> q = container(new QueryDescriptor<OptionSetDetailInfo>());

            return _repository.QueryPaged(q);
        }
    }
}
