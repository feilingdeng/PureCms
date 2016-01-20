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
        private static readonly IDataProvider<LogInfo> _repository = DataProviderFactory.GetInstance<LogInfo>(DataProvider.MSSQL);

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

        public long Count(QueryDescriptor<LogInfo> q)
        {
            ExecuteContext<LogInfo> ctx = PocoHelper.ParseContext<LogInfo>(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }

        public List<LogInfo> Top(QueryDescriptor<LogInfo> q)
        {
            ExecuteContext<LogInfo> ctx = PocoHelper.ParseContext<LogInfo>(q);
            var result = _repository.TopAsync(ctx);
            var datas = result.Result;
            if (datas != null && datas.Count() > 0)
            {
                return datas.ToList();
            }
            return null;
        }

        public PagedList<LogInfo> Query(QueryDescriptor<LogInfo> q)
        {
            ExecuteContext<LogInfo> ctx = PocoHelper.ParseContext<LogInfo>(q);
            var result = _repository.QueryPagedAsync(ctx);
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


        //#region Utilities
        //private Sql ParseSql(QueryDescriptor<LogInfo> q, Sql otherCondition = null, bool isCount = false)
        //{
        //    var columns = PocoHelper.GetSelectColumns(MetaData, q.Columns, isCount);
        //    Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName + " left join users on " + TableName + ".userid = users.userid");
        //    //过滤条件
        //    query.Append(PocoHelper.GetConditions<LogInfo>(q, otherCondition));
        //    //其它条件
        //    if (otherCondition != null)
        //    {
        //        query.Append("AND");
        //        query.Append(otherCondition);
        //    }
        //    //排序
        //    if (isCount == false)
        //    {
        //        query.Append(PocoHelper.GetOrderBy<LogInfo>(MetaData, q.SortingDescriptor));
        //    }

        //    return query;
        //}

        //private ExecuteContext<LogInfo> ParseContext(QueryDescriptor<LogInfo> q, Sql otherCondition = null, bool isCount = false)
        //{
        //    ExecuteContext<LogInfo> ctx = new ExecuteContext<LogInfo>()
        //    {
        //        ExecuteContainer = ParseSql(q, otherCondition,isCount)
        //        ,
        //        PagingInfo = q.PagingDescriptor
        //        //,
        //        //SortingInfo = q.SortingDescriptor
        //        ,
        //        TopCount = q.TopCount
        //    };

        //    return ctx;
        //}
        //#endregion
    }
}
