﻿using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Cms
{
    public class SiteRepository : ISiteRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(SiteInfo));
        private static readonly IDataProvider<SiteInfo> _repository = DataProviderFactory<SiteInfo>.GetInstance(DataProvider.MSSQL);

        public SiteRepository()
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
        public int Create(SiteInfo entity)
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
        public bool Update(SiteInfo entity)
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
        public long Count(SiteQueryContext q)
        {
            ExecuteContext<SiteInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<SiteInfo> Query(SiteQueryContext q)
        {
            ExecuteContext<SiteInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<SiteInfo> list = new PagedList<SiteInfo>()
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
        public List<SiteInfo> GetAll(SiteQueryContext q)
        {
            ExecuteContext<SiteInfo> ctx = ParseQueryContext(q);
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
        public SiteInfo GetById(int id)
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
        private Sql ParseSelectSql(SiteQueryContext q, bool isCount = false)
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
        private Sql ParseWhereSql(SiteQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.IsDefault.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsDefault=@0", optName, TableName), q.IsDefault.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.Name.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Name LIKE @0", optName, TableName), "%" + q.Name + "%");
                optName = " AND ";
            }
            if (q.IsEnabled.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsEnabled=@0", optName, TableName), q.IsEnabled.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.Theme.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Theme=@0", optName, TableName), q.Theme);
                optName = " AND ";
            }
            if (q.Url.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Url=@0", optName, TableName), q.Url);
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
        private Sql ParseQuerySql(SiteQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount))
                .Append(ParseWhereSql(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<SiteInfo>(MetaData, q.SortingDescriptor));
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
        private ExecuteContext<SiteInfo> ParseQueryContext(SiteQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<SiteInfo> ctx = new ExecuteContext<SiteInfo>()
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
