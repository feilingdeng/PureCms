using PureCms.Core.Context;
using PureCms.Core.Domain;
using PetaPoco;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PureCms.Core.Data
{
    /// <summary>
    /// mssql 数据库操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsSqlProvider<T> : IDataProvider<T> where T : new()//BaseEntity
    {
        /// <summary>
        /// 查询上下文
        /// </summary>
        public IExecuteContext<T> CurrentContext { get; private set; }
        /// <summary>
        /// 最大查询数
        /// </summary>
        public int MaxSearchCount = 25000;
        protected Database DbContext
        {
            get
            {
                //string connectionString = ConfigurationManager.ConnectionStrings["sqlconnectionstring"].ConnectionString;
                return new PetaPoco.Database("mssqlconnectionstring");
            }
        }

        public object Client
        {
            get
            {
                return DbContext;
            }
        }
        public MsSqlProvider(){
            DbContext.OpenSharedConnection();
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


        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <returns></returns>
        public virtual Task<bool> ExistsAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                var result = DbContext.FirstOrDefault<T>(GetExecuteContainer());
                return result != null;
            }
            );
        }
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <returns></returns>
        public virtual Task<long> CountAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
                {
                    long count = DbContext.ExecuteScalar<long>(GetExecuteContainer());
                    return count;
                }
            );
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<object> CreateAsync(T entity)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                object newid = DbContext.Insert(entity);
                return newid;
            }
            );
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual Task<bool> CreateManyAsync(IEnumerable<T> entities)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DbContext.BeginTransaction();
                foreach (var item in entities)
                {
                    DbContext.Insert(item);
                }
                DbContext.CompleteTransaction();
                return true;
            }
            );
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual Task<bool> UpdateAsync(T entity)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int result = DbContext.Update(entity);
                return result > 0;
            }
            );
        }

        /// <summary>
        /// 批量更新记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual Task<bool> UpdateManyAsync(IEnumerable<T> entities)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DbContext.BeginTransaction();
                foreach (var item in entities)
                {
                    DbContext.Update(item);
                }
                DbContext.CompleteTransaction();
                return true;
            }
            );
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<bool> DeleteAsync(object id)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                int result = DbContext.Delete<T>(id);
                return result > 0;
            }
            );
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual Task<bool> DeleteManyAsync(IEnumerable<object> ids)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                DbContext.BeginTransaction();
                foreach (var id in ids)
                {
                    DbContext.Delete<T>(id);
                }
                DbContext.CompleteTransaction();
                return true;
            }
            );
        }

        /// <summary>
        /// 根据ID获取记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<T> FindByIdAsync(object id)
        {
            return System.Threading.Tasks.Task.Run(() =>
                {
                    return DbContext.SingleOrDefault<T>(id);
                }
            );
        }

        /// <summary>
        /// 根据条件获取一条记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<T> SingleAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                return DbContext.FirstOrDefault<T>(GetExecuteContainer());
            }
            );
        }
        
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <returns></returns>
        public virtual Task<Page<T>> QueryPagedAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                Page<T> result = null;
                if (null != context.PagingInfo)
                {
                    PageDescriptor p = context.PagingInfo;
                    result = DbContext.Page<T>(p.PageNumber, p.PageSize, GetExecuteContainer());
                }
                else
                {
                    result = DbContext.Page<T>(1, context.TopCount > 0 ? context.TopCount : MaxSearchCount, GetExecuteContainer());
                }

                return result;
            }
            );
        }

        /// <summary>
        /// 查询前N条记录
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> TopAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                IEnumerable<T> result = null;
                result = DbContext.SkipTake<T>(0, context.TopCount > 0 ? context.TopCount : MaxSearchCount, GetExecuteContainer());

                return result;
            }
            );
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="context">查询上下文</param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> QueryAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                IEnumerable<T> result = null;
                result = DbContext.Query<T>(GetExecuteContainer());

                return result;
            }
            );
        }

        /// <summary>
        /// 根据条件更新记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<bool> UpdateByQueryAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                int result = DbContext.Execute(GetExecuteContainer());
                return result > 0;
            }
            );
        }
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task<bool> DeleteByQueryAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                int result = DbContext.Execute(GetExecuteContainer());
                return result > 0;
            }
            );
        }

        /// <summary>
        /// 直接执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<int> ExecuteAsync(IExecuteContext<T> context)
        {
            this.CurrentContext = context;
            return System.Threading.Tasks.Task.Run(() =>
            {
                var result = DbContext.Execute(GetExecuteContainer());
                return result;
            }
            );
        }

        public Sql GetExecuteContainer()
        {
            Sql s = this.CurrentContext.ExecuteContainer as Sql;
            return s;
        }
    }
}
