using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Cms
{
    public class ArticleRepository : IArticleRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(ArticleInfo));
        private static readonly IDataProvider<ArticleInfo> _repository = DataProviderFactory<ArticleInfo>.GetInstance(DataProvider.MSSQL);//new MsSqlProvider<ArticleInfo>();

        public ArticleRepository()
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
        public long Create(ArticleInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            long id = long.Parse(result.Result.ToString());
            return id;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(ArticleInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(long id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteById(List<long> ids)
        {
            List<object> idss = ids.Select(x => x as object).ToList();
            var result = _repository.DeleteManyAsync(idss);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="q">过滤条件</param>
        /// <returns></returns>
        public bool Update(List<KeyValuePair<string, object>> sets, ArticleQueryContext q)
        {
            Guard.ArgumentNotNullOrEmpty<KeyValuePair<string, object>>(sets, "sets");

            Sql query = PetaPoco.Sql.Builder.Append("UPDATE " + TableName + " SET ");
            string optName = string.Empty;
            foreach (var item in sets)
            {
                query.Append(TableName+"." + item.Key + "=@0", item.Value);
            }
            query.Append(ParseWhereSql(q));
            int result = ((Database)_repository.Client).Execute(query);
            return result > 0;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(ArticleQueryContext q)
        {
            ExecuteContext<ArticleInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<ArticleInfo> Query(ArticleQueryContext q)
        {
            ExecuteContext<ArticleInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<ArticleInfo> list = new PagedList<ArticleInfo>()
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
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleInfo GetById(long id)
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
        private Sql ParseSelectSql(ArticleQueryContext q, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        /// <summary>
        /// 根据上下文生成过滤条件语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <returns></returns>
        private Sql ParseWhereSql(ArticleQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.ArticleIdList != null && q.ArticleIdList.Any())
            {
                filter.Append(string.Format("{0} {1}.ArticleId IN(@0)", optName, TableName), q.ArticleIdList);
                optName = " AND ";
            }
            if (q.CategoryId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ArticleCategoryId=@0", optName, TableName), q.CategoryId.Value);
                optName = " AND ";
            }
            if (q.Author.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Author=@0", optName, TableName), q.Author);
                optName = " AND ";
            }
            if (q.IsShow.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsShow=@0", optName, TableName), q.IsShow.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.Title.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Title LIKE @0", optName, TableName), "%" + q.Title + "%");
                optName = " AND ";
            }
            if (q.BeginTime.HasValue)
            {
                filter.Append(string.Format("{0} {1}.CreatedOn>=@0", optName, TableName), q.BeginTime.Value);
                optName = " AND ";
            }
            if (q.EndTime.HasValue)
            {
                filter.Append(string.Format("{0} {1}.CreatedOn<=@0", optName, TableName), q.EndTime.Value);
                optName = " AND ";
            }
            if (filter.SQL.IsNotEmpty())
            {
                query.Append("WHERE ");
                query.Append(filter);
            }
            //其它条件
            if (otherCondition != null)
            {
                query.Append(optName);
                query.Append(otherCondition);
            }
            return query;
        }
        /// <summary>
        /// 根据上下文生成查询语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private Sql ParseQuerySql(ArticleQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount));
            query.Append(ParseWhereSql(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<ArticleInfo>(MetaData, q.SortingDescriptor));
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
        private ExecuteContext<ArticleInfo> ParseQueryContext(ArticleQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<ArticleInfo> ctx = new ExecuteContext<ArticleInfo>()
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
