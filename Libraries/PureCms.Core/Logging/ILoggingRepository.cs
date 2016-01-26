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
        int Create(LogInfo entity);
        

        bool DeleteById(int id);

        PagedList<LogInfo> QueryPaged(QueryDescriptor<LogInfo> q);
        List<LogInfo> Top(QueryDescriptor<LogInfo> q);

        LogInfo FindById(int id);
    }
}
