using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Cms
{
    public class ThemeRepository : IThemeRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(ThemeInfo));
        private static readonly IDataProvider<ThemeInfo> _repository = DataProviderFactory<ThemeInfo>.GetInstance(DataProvider.MSSQL);

        public ThemeRepository()
        {
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
        #region implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(ThemeInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(ThemeInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<ThemeInfo> q)
        {
            ExecuteContext<ThemeInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 分页查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<ThemeInfo> Query(QueryDescriptor<ThemeInfo> q)
        {
            ExecuteContext<ThemeInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<ThemeInfo> list = new PagedList<ThemeInfo>()
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
        /// 查询所有记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<ThemeInfo> GetAll(QueryDescriptor<ThemeInfo> q)
        {
            ExecuteContext<ThemeInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetAllAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return null;
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ThemeInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        } 
        #endregion


        #region Utilities
        /// <summary>
        /// 根据上下文生成查询语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private Sql ParseSelectSql(QueryDescriptor<ThemeInfo> q, bool isCount = false)
        {
            var columns = PocoHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        /// <summary>
        /// 根据上下文生成查询语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private Sql ParseQuerySql(QueryDescriptor<ThemeInfo> q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount));
            //过滤条件
            query.Append(PocoHelper.GetConditions<ThemeInfo>(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(PocoHelper.GetOrderBy<ThemeInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }
        /// <summary>
        /// 转换为数据库上下文
        /// </summary>
        /// <param name="q">实体上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private ExecuteContext<ThemeInfo> ParseQueryContext(QueryDescriptor<ThemeInfo> q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<ThemeInfo> ctx = new ExecuteContext<ThemeInfo>()
            {
                ExecuteContainer = ParseQuerySql(q, otherCondition, isCount)
                ,
                PagingInfo = q.PagingDescriptor
                ,
                TopCount = q.TopCount
            };

            return ctx;
        }
        #endregion
    }
}
