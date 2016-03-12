using PureCms.Core.Context;
using PureCms.Core.Domain;
using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace PureCms.Core.Data
{
    /// <summary>
    /// mysql 数据库操作类（暂未实现）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MySqlProvider<T> : IDataProvider<T> where T : new()//BaseEntity
    {
        public object Client
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Task<long> CountAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<object> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateManyAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByQueryAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IEnumerable<object> ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task<T> SingleAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> QueryAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<Page<T>> QueryPagedAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> TopAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateByQueryAsync(IExecuteContext<T> context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateManyAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CompleteTransaction()
        {
            throw new NotImplementedException();
        }

        public void AbortTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
