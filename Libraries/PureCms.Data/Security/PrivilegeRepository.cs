using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Data.Security
{
    public class PrivilegeRepository : IPrivilegeRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(PrivilegeInfo));
        private static readonly IDataProvider<PrivilegeInfo> _repository = DataProviderFactory<PrivilegeInfo>.GetInstance(DataProvider.MSSQL);

        private string TableName{
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        public PrivilegeRepository()
        {
            //PetaPoco.Database.Mapper = new ColumnMapper();
        }
        #region Implements
        public int Create(PrivilegeInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }

        public bool Update(PrivilegeInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }

        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        public long Count(PrivilegeQueryContext q)
        {
            ExecuteContext<PrivilegeInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }


        public int MoveNode(int moveid, int targetid, int parentid, string position)
        {
            int result = 0;
            var moveNode = GetById(moveid);
            var targetNode = GetById(targetid);
            Sql s = Sql.Builder;
            switch (position)
            {
                case "after":
                    if (moveNode.ParentPrivilegeId == targetNode.ParentPrivilegeId)
                    {
                        //移动节点序号等于目标节点的序号+1
                        s.Append("UPDATE Privileges SET DisplayOrder=" + (targetNode.DisplayOrder + 1) + " WHERE PrivilegeId=@0;", moveid);
                    }
                    break;
                case "inside":
                    if (moveNode.ParentPrivilegeId == targetid)
                    {
                        //移动节点排第一，其它的排序+1
                        s.Append("UPDATE Privileges SET DisplayOrder=0 WHERE PrivilegeId=@0;", moveid);
                    }
                    break;
                default:
                    result = -1;
                    break;
            }
            if (s.SQL.IsNotEmpty())
            {
                //重新排序
                s.Append("SELECT IDENTITY(INT,1,1) AS displayorder,PrivilegeId*1 AS PrivilegeId INTO #tmp");
                s.Append("FROM [Privileges] WHERE ParentPrivilegeId=@0 ORDER BY displayorder", moveNode.ParentPrivilegeId);
                s.Append("UPDATE a SET DisplayOrder = b.displayorder FROM [Privileges] a");
                s.Append("INNER JOIN #tmp b ON a.PrivilegeId=b.PrivilegeId");
                s.Append("DROP TABLE #tmp");
                ((Database)_repository.Client).Execute(s);
                result = 1;
            }
            return result;
            //SqlParameter[] ps = new SqlParameter[]{
            //    new SqlParameter() { SqlDbType = SqlDbType.Int,Value = moveid }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.Int, Value = targetid }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.NVarChar,Value = position }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.Int,Value = 3 }
            //    ,new SqlParameter() { Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int }
            //};

            //((Database)_repository.Client).Execute("EXEC [usp_Security_UpdPrivilegeNode] @0,@1,@2,@3,@4 OUTPUT", ps);

            //return (int)ps[4].Value;
        }
        public PagedList<PrivilegeInfo> Query(PrivilegeQueryContext q)
        {
            ExecuteContext<PrivilegeInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<PrivilegeInfo> list = new PagedList<PrivilegeInfo>()
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

        public PrivilegeInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        }
        public PrivilegeInfo GetOne(PrivilegeQueryContext q)
        {
            ExecuteContext<PrivilegeInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }
        public List<PrivilegeInfo> GetAll(PrivilegeQueryContext q)
        {
            ExecuteContext<PrivilegeInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetAllAsync(ctx);
            if (result.Result != null)
            {
                return result.Result.ToList();
            }
            return null;
        }
        
        #endregion

        #region Utilities
        private Sql ParseSelectSql(PrivilegeQueryContext q, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        private Sql ParseWhereSql(PrivilegeQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.ParentPrivilegeId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ParentPrivilegeId=@0", optName, TableName), q.ParentPrivilegeId.Value);
                optName = " AND ";
            }
            if (q.Level.HasValue)
            {
                filter.Append(string.Format("{0} {1}.Level=@0", optName, TableName), q.Level.Value);
                optName = " AND ";
            }
            if (q.Url.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Url LIKE @0", optName, TableName), "%" + q.Url + "%");
                optName = " AND ";
            }
            if (q.ClassName.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.ClassName=@0", optName, TableName), q.ClassName);
                optName = " AND ";
            }
            if (q.MethodName.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.MethodName=@0", optName, TableName), q.MethodName);
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
        private Sql ParseQuerySql(PrivilegeQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q,isCount));
            query.Append(ParseWhereSql(q,otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<PrivilegeInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }

        private Sql ParseUpdateSql(PrivilegeQueryContext q, Sql sets, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder.Append("UPDATE " + TableName);

            if (sets.SQL.IsNotEmpty())
            {
                query.Append(" SET ");
                query.Append(sets);
            }

            query.Append(ParseWhereSql(q, otherCondition));

            return query;
        }

        private ExecuteContext<PrivilegeInfo> ParseQueryContext(PrivilegeQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<PrivilegeInfo> ctx = new ExecuteContext<PrivilegeInfo>()
            {
                ExecuteContainer = ParseQuerySql(q, otherCondition, isCount)
                ,
                PagingInfo = q.PagingDescriptor
                ,
                TopCount = q.TopCount
            };

            return ctx;
        }
        private ExecuteContext<PrivilegeInfo> ParseUpdateContext(PrivilegeQueryContext q, Sql sets, Sql otherCondition = null)
        {
            ExecuteContext<PrivilegeInfo> ctx = new ExecuteContext<PrivilegeInfo>()
            {
                ExecuteContainer = ParseUpdateSql(q, sets, otherCondition)
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
