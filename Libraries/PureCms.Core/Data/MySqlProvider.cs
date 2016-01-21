using PureCms.Core.Context;
using PureCms.Core.Domain;
using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    /// <summary>
    /// mysql 数据库操作类（暂未实现）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MySqlProvider<T> : IDataProvider<T> where T : BaseEntity
    {
        /// <summary>
        /// 查询上下文
        /// </summary>
        public IExecuteContext<T> CurrentQueryContext { get; private set; }
        /// <summary>
        /// 最大查询数
        /// </summary>
        public int MaxSearchCount = 25000;
        protected Database DbContext
        {
            get
            {
                return new PetaPoco.Database("mysqlconnectionstring");
            }
        }

        public object Client
        {
            get
            {
                return DbContext;
            }
        }
        public virtual void BeginTransaction()
        {
            DbContext.BeginTransaction();
        }
        public virtual void CompleteTransaction()
        {
            DbContext.CompleteTransaction();
        }
        public virtual void AbortTransaction()
        {
            DbContext.AbortTransaction();
        }


        public Task<bool> ExistsAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<long> CountAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> CreateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CreateManyAsync(IEnumerable<T> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateManyAsync(IEnumerable<T> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateByQueryAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IEnumerable<object> ids)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteByQueryAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetSingleAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> SearchAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<Page<T>> PagedAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> TopAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(IExecuteContext<T> q)
        {
            throw new System.NotImplementedException();
        }
    }
}
