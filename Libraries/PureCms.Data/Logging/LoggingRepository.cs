using System.Collections.Generic;
using PureCms.Core.Logging;
using PureCms.Core.Domain.Logging;
using PureCms.Core.Context;

namespace PureCms.Data.Logging
{
    public class LoggingRepository : ILoggingRepository
    {
        private static readonly DataRepository<LogInfo> _repository = new DataRepository<LogInfo>();

        /// <summary>
        /// 实体元数据
        /// </summary>
        private PetaPoco.Database.PocoData MetaData
        {
            get
            {
                return _repository.MetaData;
            }
        }
        /// <summary>
        /// 实体表名
        /// </summary>
        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        #region Implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(LogInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {
            return _repository.Delete(id);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<LogInfo> q)
        {
            return _repository.Count(q);
        }

        public List<LogInfo> Top(QueryDescriptor<LogInfo> q)
        {
            return _repository.Top(q);
        }

        public PagedList<LogInfo> QueryPaged(QueryDescriptor<LogInfo> q)
        {
            return _repository.QueryPaged(q);
        }

        public LogInfo FindById(int id)
        {
            return _repository.FindById(id);
        } 
        #endregion
    }
}
