using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Security
{
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(RoleInfo));
        private static readonly IDataProvider<RoleInfo> _repository = DataProviderFactory<RoleInfo>.GetInstance(DataProvider.MSSQL);

        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        public RoleRepository()
        {
        }

        #region Implements
        public int Create(RoleInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }

        public bool Update(RoleInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }

        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        public long Count(RoleQueryContext q)
        {
            ExecuteContext<RoleInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        public PagedList<RoleInfo> Query(RoleQueryContext q)
        {
            ExecuteContext<RoleInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<RoleInfo> list = new PagedList<RoleInfo>()
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

        public RoleInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        }
        public List<RoleInfo> GetAll(RoleQueryContext q)
        {
            ExecuteContext<RoleInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetAllAsync(ctx);
            if (result.Result != null)
            {
                return result.Result.ToList();
            }
            return null;
        }
        #endregion

        #region Utilities
        private Sql ParseSelectSql(RoleQueryContext q, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        private Sql ParseWhereSql(RoleQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.ParentRoleId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ParentRoleId=@0", optName, TableName), q.ParentRoleId.Value);
                optName = " AND ";
            }
            if (q.RoleId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.RoleId=@0", optName, TableName), q.RoleId.Value);
                optName = " AND ";
            }
            if (q.IsEnabled.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsEnabled=@0", optName, TableName), q.IsEnabled.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.Name.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Name LIKE @0", optName, TableName), "%" + q.Name + "%");
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
        private Sql ParseQuerySql(RoleQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount));
            query.Append(ParseWhereSql(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<RoleInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }

        private ExecuteContext<RoleInfo> ParseQueryContext(RoleQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<RoleInfo> ctx = new ExecuteContext<RoleInfo>()
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
