using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCms.Core.Domain.Logging;
using PureCms.Core.Context;

namespace PureCms.Core.Logging
{
    public interface ILoggingRepository
    {
        long Create(LogInfo entity);

        bool Update(LogInfo entity);

        bool DeleteById(long id);

        PagedList<LogInfo> Query(QueryDescriptor<LogInfo> q);

        LogInfo GetById(long id);
    }
}
