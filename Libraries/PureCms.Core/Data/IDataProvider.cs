using PureCms.Core.Context;
using PureCms.Core.Domain;
using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    public interface IDataProvider<T>
     where T : BaseEntity
    {
        object Client { get; }
        Task<bool> ExistsAsync(IExecuteContext<T> q);
        Task<long> CountAsync(IExecuteContext<T> q);
        Task<object> CreateAsync(T entity);
        Task<bool> CreateManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> UpdateManyAsync(IEnumerable<T> entities);
        Task<bool> UpdateByQueryAsync(IExecuteContext<T> q);
        Task<bool> DeleteAsync(object id);
        Task<bool> DeleteManyAsync(IEnumerable<object> ids);
        Task<bool> DeleteByQueryAsync(IExecuteContext<T> q);
        Task<T> GetByIdAsync(object id);
        Task<T> GetSingleAsync(IExecuteContext<T> q);
        Task<IEnumerable<T>> SearchAsync(IExecuteContext<T> q);
        Task<Page<T>> PagedAsync(IExecuteContext<T> q);
        Task<IEnumerable<T>> TopAsync(IExecuteContext<T> q);
        Task<IEnumerable<T>> GetAllAsync(IExecuteContext<T> q);
    }
}
