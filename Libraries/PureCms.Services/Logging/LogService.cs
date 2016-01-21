using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCms.Core.Logging;
using PureCms.Core.Domain.Logging;
using PureCms.Data;
using PureCms.Data.Logging;
using PureCms.Core.Context;

namespace PureCms.Services.Logging
{
    public class LogService
    {
        ILoggingRepository _loggingRepository = new LoggingRepository();


        public long Create(LogInfo entity)
        {
            return _loggingRepository.Create(entity);
        }

        public PagedList<LogInfo> Query(Func<LogQueryContext, LogQueryContext> container)
        {
            LogQueryContext q = container(new LogQueryContext());

            return _loggingRepository.Query(q);
        }
    }
}
