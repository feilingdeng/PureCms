using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Security
{
    public class RolePrivilegesRepository : IRolePrivilegesRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(RolePrivilegesInfo));
        private static readonly IDataProvider<RolePrivilegesInfo> _repository = DataProviderFactory<RolePrivilegesInfo>.GetInstance(DataProvider.MSSQL);

        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        public RolePrivilegesRepository()
        {
        }
        #region Implements
        public int Create(RolePrivilegesInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }

        public bool CreateMany(List<RolePrivilegesInfo> entities)
        {
            var result = _repository.CreateManyAsync(entities);
            return result.Result;
        }

        public bool Update(RolePrivilegesInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }

        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }

        public bool DeleteByRoleId(int roleId)
        {
            Sql s = Sql.Builder.Append("DELETE " + TableName + " WHERE ");
            s.Append("RoleId=@0", roleId);
            ExecuteContext<RolePrivilegesInfo> ctx = new ExecuteContext<RolePrivilegesInfo>()
            {
                ExecuteContainer = s
            };
            var result = _repository.DeleteByQueryAsync(ctx);
            return result.Result;
        }
        public long Count(RolePrivilegesQueryContext q)
        {
            ExecuteContext<RolePrivilegesInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        public PagedList<RolePrivilegesInfo> Query(RolePrivilegesQueryContext q)
        {
            ExecuteContext<RolePrivilegesInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<RolePrivilegesInfo> list = new PagedList<RolePrivilegesInfo>()
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

        public RolePrivilegesInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        }
        public List<RolePrivilegesInfo> GetAll(RolePrivilegesQueryContext q)
        {
            ExecuteContext<RolePrivilegesInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetAllAsync(ctx);
            if (result.Result != null)
            {
                return result.Result.ToList();
            }
            return null;
        }

        #endregion

        #region Utilities
        private Sql ParseSelectSql(RolePrivilegesQueryContext q, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            query.Append("INNER JOIN Roles ON " + TableName + ".RoleId=Roles.RoleId");
            query.Append("INNER JOIN Privileges ON " + TableName + ".PrivilegeId=Privileges.PrivilegeId");
            return query;
        }
        private Sql ParseWhereSql(RolePrivilegesQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.RoleId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.RoleId=@0", optName, TableName), q.RoleId.Value);
                optName = " AND ";
            }
            if (q.PrivilegeId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.PrivilegeId=@0", optName, TableName), q.PrivilegeId.Value);
                optName = " AND ";
            }
            if (q.RolePrivilegeId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.RolePrivilegeId=@0", optName, TableName), q.RolePrivilegeId.Value);
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
        private Sql ParseQuerySql(RolePrivilegesQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount));
            query.Append(ParseWhereSql(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<RolePrivilegesInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }

        private ExecuteContext<RolePrivilegesInfo> ParseQueryContext(RolePrivilegesQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<RolePrivilegesInfo> ctx = new ExecuteContext<RolePrivilegesInfo>()
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
