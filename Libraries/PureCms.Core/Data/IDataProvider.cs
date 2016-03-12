using PureCms.Core.Context;
using PureCms.Core.Domain;
using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    public interface IDataProvider<T>
     where T : new()//BaseEntity
    {
        object Client { get; set; }
        void BeginTransaction();
        void CompleteTransaction();
        void AbortTransaction();
        Task<bool> ExistsAsync(IExecuteContext<T> context);
        Task<long> CountAsync(IExecuteContext<T> context);
        Task<object> CreateAsync(T entity);
        Task<bool> CreateManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateByQueryAsync(IExecuteContext<T> context);
        Task<bool> DeleteAsync(object id);
        Task<bool> DeleteManyAsync(IEnumerable<object> ids);
        Task<bool> DeleteByQueryAsync(IExecuteContext<T> context);
        Task<T> FindByIdAsync(object id);
        Task<T> SingleAsync(IExecuteContext<T> context);
        Task<Page<T>> QueryPagedAsync(IExecuteContext<T> context);
        Task<IEnumerable<T>> TopAsync(IExecuteContext<T> context);
        Task<IEnumerable<T>> QueryAsync(IExecuteContext<T> context);

        Task<int> ExecuteAsync(IExecuteContext<T> context);
    }
}
