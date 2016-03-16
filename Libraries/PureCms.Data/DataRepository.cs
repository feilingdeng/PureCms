using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PureCms.Core.Caching;
using System.Data;

namespace PureCms.Data
{
    public class DataRepository<T> where T : new()//BaseEntity
    {
        private readonly IDataProvider<T> _repository = DataProviderFactory.GetInstance<T>(DataProvider.MSSQL);
        private readonly AspNetCache _cache = new AspNetCache();
        private readonly Type EntityType = typeof(T);
        /// <summary>
        /// 实体元数据
        /// </summary>
        public PetaPoco.Database.PocoData MetaData {
            get
            {
                //从缓存获取
                if (_cache.Contains(EntityType.FullName))
                {
                    return _cache.Get(EntityType.FullName) as PetaPoco.Database.PocoData;
                }

                //var md = new PetaPoco.Database.PocoData(EntityType);
                var md = PetaPoco.Database.PocoData.ForType(EntityType);
                _cache.Set(EntityType.FullName, md, null);
                return md;
            }
        }

        public DataRepository(){
            //PetaPoco.Database.Mapper = new ColumnMapper();
        }
        public DataRepository(IDbConnection connection)
        {
            _repository.Client = new PetaPoco.Database(connection);
            //(_repository.Client as PetaPoco.Database).Connection = connection;
            //PetaPoco.Database.Mapper = new ColumnMapper();
        }
        public IDbConnection GetConnection()
        {
            return (_repository.Client as PetaPoco.Database).Connection;
        }
        public virtual void BeginTransaction()
        {
            _repository.BeginTransaction();
        }
        public virtual void CompleteTransaction()
        {
            _repository.CompleteTransaction();
        }
        public virtual void RollBackTransaction()
        {
            _repository.AbortTransaction();
        }
        #region 创建记录
        /// <summary>
        /// 创建一行记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>记录主键值</returns>
        public int Create(T entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }
        /// <summary>
        /// 创建一行记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>是否成功</returns>
        public bool CreateObject(T entity)
        {
            var result = _repository.CreateAsync(entity);
            object val = result.Result;


            return (bool)val;
        }
        /// <summary>
        /// 批量创建记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool CreateMany(List<T> entities)
        {
            var result = _repository.CreateManyAsync(entities);
            return result.Result;
        }
        #endregion

        #region 删除记录
        /// <summary>
        /// 删除一行记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool Delete(object id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids">主键</param>
        /// <returns></returns>
        public bool Delete(List<object> ids)
        {
            //List<object> idss = ids.Select(x => x as object).ToList();
            var result = _repository.DeleteManyAsync(ids);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> predicate)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate);
            Sql s = Sql.Builder.Append("DELETE [" + MetaData.TableInfo.TableName + "] ")
                .Append(PocoHelper.GetConditions(q.QueryText, q.Parameters));
            ExecuteContext<T> ctx = new ExecuteContext<T>(s);
            var result = _repository.DeleteByQueryAsync(ctx);
            return result.Result;
        }
        #endregion

        #region 更新记录
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
        #endregion

        #region 查询记录
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
        /// <returns></returns>
        public List<T> FindAll()
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, null);
            var result = _repository.QueryAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return new List<T>();
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
            return new List<T>();
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
            return new List<T>();
        }
        /// <summary>
        /// 查询前N条记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<T> Top(QueryDescriptor<T> q)
        {
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(q);
            var result = _repository.TopAsync(ctx);
            var datas = result.Result;
            if (datas != null)
            {
                return datas.ToList();
            }
            return new List<T>();
        }

        /// <summary>
        /// 查询前N条记录
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <param name="sorts">排序</param>
        /// <returns></returns>
        public List<T> Top(int top, Expression<Func<T, bool>> predicate, params Func<SortDescriptor<T>, SortDescriptor<T>>[] sorts)
        {
            QueryDescriptor<T> q = new QueryDescriptor<T>();
            q.Where(predicate)
                .Sort(sorts);
            ExecuteContext<T> ctx = PocoHelper.ParseContext<T>(MetaData, q);
            ctx.TopCount = top;
            var result = _repository.TopAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return new List<T>();
        }
        /// <summary>
        /// 查询一行记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public T FindById(object id)
        {
            var result = _repository.FindByIdAsync(id);
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
            var result = _repository.SingleAsync(ctx);
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
        #endregion

        #region 是否存在
        /// <summary>
        /// 是否存在记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Exists(QueryDescriptor<T> context)
        {
            Sql s = Sql.Builder.Append("SELECT 1 AS result FROM [" + MetaData.TableInfo.TableName + "] ")
                .Append(PocoHelper.GetConditions(context));
            ExecuteContext<T> ctx = new ExecuteContext<T>(s);
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
        /// 是否存在记录
        /// </summary>
        /// <param name="s">上下文</param>
        /// <returns></returns>
        public bool Exists(Sql s)
        {
            ExecuteContext<T> ctx = new ExecuteContext<T>(s);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        #endregion

        #region 执行
        /// <summary>
        /// 直接执行
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int Execute(Sql s)
        {
            ExecuteContext<T> ctx = new ExecuteContext<T>();
            ctx.ExecuteContainer = s;
            var result = _repository.ExecuteAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 直接执行查询语句
        /// </summary>
        /// <param name="predicate">过滤条件</param>
        /// <param name="sorts">排序</param>
        /// <returns></returns>
        public List<T> ExecuteQuery(Sql s)
        {
            ExecuteContext<T> ctx = new ExecuteContext<T>();
            ctx.ExecuteContainer = s;
            var result = _repository.QueryAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return new List<T>();
        }
        /// <summary>
        /// 直接执行查询语句
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<T> ExecuteQuery(string s, params object[] args)
        {
            var result = ((PetaPoco.Database)_repository.Client).Query<T>(s, args);
            return result.AsEnumerable().ToList();
        }
        /// <summary>
        /// 直接执行查询语句
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public PagedList<T> ExecuteQueryPaged(int page, int pageSize, string s, params object[] args)
        {
            var result = ((PetaPoco.Database)_repository.Client).Page<T>(page, pageSize, s);
            var p = new PagedList<T>();
            result.CopyTo(p);
            return p;
        }
        #endregion
    }
}
