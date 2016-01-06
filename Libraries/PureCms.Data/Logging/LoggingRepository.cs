using System.Collections.Generic;
using System.Linq;
using PureCms.Core;
using PureCms.Core.Logging;
using PureCms.Core.Data;
using PureCms.Core.Domain.Logging;
using PureCms.Core.Context;
using PetaPoco;
using System;

namespace PureCms.Data.Logging
{
    public class LoggingRepository : ILoggingRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(LogInfo));
        private static readonly IDataProvider<LogInfo> _repository = DataProviderFactory<LogInfo>.GetInstance(DataProvider.MSSQL);

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
        #region Implements
        public long Create(LogInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            long id = long.Parse(result.Result.ToString());
            return id;
        }

        public bool Update(LogInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }

        public bool DeleteById(long id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }

        public long Count(LogQueryContext q)
        {
            ExecuteContext<LogInfo> ctx = ParseContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }

        public List<LogInfo> Top(LogQueryContext q)
        {
            ExecuteContext<LogInfo> ctx = ParseContext(q);
            var result = _repository.TopAsync(ctx);
            var datas = result.Result;
            if (datas != null && datas.Count() > 0)
            {
                return datas.ToList();
            }
            return null;
        }

        public PagedList<LogInfo> Query(LogQueryContext q)
        {
            ExecuteContext<LogInfo> ctx = ParseContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<LogInfo> list = new PagedList<LogInfo>()
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

        public LogInfo GetById(long id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        } 
        #endregion


        #region Utilities
        private Sql ParseSql(LogQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            //var columns = isCount ? "COUNT(1)" : (q.Columns != null && q.Columns.Count > 0 ? string.Join(",",q.Columns) : "log.*,users.username");
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName + " left join users on " + TableName + ".userid = users.userid");
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.ClientIP.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.ClientIP LIKE @0", optName, TableName), "%" + q.ClientIP + "%");
                optName = " AND ";
            }
            if (q.Description.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Description LIKE @0", optName, TableName), "%" + q.Description + "%");
                optName = " AND ";
            }
            if (q.Url.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Url LIKE @0", optName, TableName), "%" + q.Url + "%");
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
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<LogInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }

        private ExecuteContext<LogInfo> ParseContext(LogQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<LogInfo> ctx = new ExecuteContext<LogInfo>()
            {
                ExecuteContainer = ParseSql(q, otherCondition,isCount)
                ,
                PagingInfo = q.PagingDescriptor
                //,
                //SortingInfo = q.SortingDescriptor
                ,
                TopCount = q.TopCount
            };

            return ctx;
        }
        #endregion
    }
}
