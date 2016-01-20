using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Data
{
    public class DataRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        public readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(T));
        private readonly IDataProvider<T> _repository = DataProviderFactory.GetInstance<T>(DataProvider.MSSQL);

        /// <summary>
        /// 创建一行记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(T entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return (int)id;
        }
        /// <summary>
        /// 删除一行记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public bool Delete(List<int> ids)
        {
            List<object> idss = ids.Select(x => x as object).ToList();
            var result = _repository.DeleteManyAsync(idss);
            return result.Result;
        }

        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate);
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            var result = _repository.DeleteByQueryAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Update(UpdateContext<T> context)
        {
            Guard.ArgumentNotNullOrEmpty<KeyValuePair<string, object>>(context.Sets, "sets");

            ExecuteContext<T> ctx = new ExecuteContext<T>(PocoHelper.ParseUpdateSql<T>(MetaData, context));
            var result = _repository.UpdateByQueryAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<T> QueryPaged(QueryDescriptor<T> q)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            var result = _repository.QueryPagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<T> list = new PagedList<T>()
                {
                    CurrentPage = pageDatas.CurrentPage
                    ,
                    ItemsPerPage = pageDatas.ItemsPerPage
                    ,
                    TotalItems = pageDatas.TotalItems
                    ,
                    TotalPages = pageDatas.TotalPages
                    ,
                    Items = pageDatas.Items
                };
                return list;
            }
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public List<T> Query(QueryDescriptor<T> q)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            var result = _repository.QueryAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return null;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <param name="sorts">排序</param>
        /// <returns></returns>
        public List<T> Query(Expression<Func<T, bool>> predicate, params Func<SortDescriptor<T>, SortDescriptor<T>>[] sorts)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate)
                .Sort(sorts);
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            var result = _repository.QueryAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return null;
        }
        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T FindById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> predicate)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate);
            return Find(q);
        }
        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public T Find(QueryDescriptor<T> q)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<T> q)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Exists(QueryDescriptor<T> context)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, context);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate);
            return Exists(q);
        }
        /// <summary>
        /// 直接执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public int Execute(QueryDescriptor<T> context)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, context);
            var result = _repository.ExecuteAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 直接执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public int Execute(Sql s)
        {
            ExecuteContext<T> ctx = new ExecuteContext<T>();
            ctx.ExecuteContainer = s;
            var result = _repository.ExecuteAsync(ctx);
            return result.Result;
        }
    }
}
