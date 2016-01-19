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
        public long Count(QueryDescriptor<RoleInfo> q)
        {
            ExecuteContext<RoleInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        public PagedList<RoleInfo> Query(QueryDescriptor<RoleInfo> q)
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
        public List<RoleInfo> GetAll(QueryDescriptor<RoleInfo> q)
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
        private Sql ParseSelectSql(QueryDescriptor<RoleInfo> q, bool isCount = false)
        {
            var columns = PocoHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        private Sql ParseQuerySql(QueryDescriptor<RoleInfo> q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount));
            //过滤条件
            query.Append(PocoHelper.GetConditions<RoleInfo>(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(PocoHelper.GetOrderBy<RoleInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }

        private ExecuteContext<RoleInfo> ParseQueryContext(QueryDescriptor<RoleInfo> q, Sql otherCondition = null, bool isCount = false)
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
