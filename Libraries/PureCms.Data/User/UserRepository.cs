using PureCms.Core.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCms.Core.Domain.User;
using PureCms.Core.Data;
using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core;
using PureCms.Core.Domain;

namespace PureCms.Data.User
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(UserInfo));
        private static readonly IDataProvider<UserInfo> _repository = DataProviderFactory<UserInfo>.GetInstance(DataProvider.MSSQL);

        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        public UserRepository()
        {
        }
        #region Implements
        public bool ExistsEmail(string email, int currentUserId = 0)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.EmailAddress == email);
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q, otherCondition);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        public bool ExistsUserName(string userName, int currentUserId = 0)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.UserName == userName);
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q, otherCondition);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        public bool ExistsMobile(string mobileNumber, int currentUserId = 0)
        {

            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.MobileNumber == mobileNumber);
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q, otherCondition);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }

        public int Create(UserInfo entity)
        {
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return (int)id;
        }

        public bool Update(UserInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }

        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteById(List<int> ids)
        {
            List<object> idss = ids.Select(x => x as object).ToList();
            var result = _repository.DeleteManyAsync(idss);
            return result.Result;
        }

        public PagedList<UserInfo> Query(QueryDescriptor<UserInfo> q)
        {
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<UserInfo> list = new PagedList<UserInfo>()
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

        public UserInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
        }

        public UserInfo GetByEmail(string email)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.EmailAddress == email);
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByUserName(string userName)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.UserName == userName);
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByMobile(string mobileNumber)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.MobileNumber == mobileNumber);
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByLoginName(string loginName)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w => w.LoginName == loginName);
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByLoginNameAndPassword(string loginName, string password)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            q.Where(w=>w.LoginName == loginName && w.Password == password);
            ExecuteContext<UserInfo> ctx = PocoHelper.ParseContext<UserInfo>(ParseSql, q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="q">过滤条件</param>
        /// <returns></returns>
        public bool Update(UpdateContext<UserInfo> q)
        {
            Guard.ArgumentNotNullOrEmpty<KeyValuePair<string, object>>(q.Sets, "sets");

            Sql query = PetaPoco.Sql.Builder.Append("UPDATE " + TableName + " SET ");
            string optName = string.Empty;
            foreach (var item in q.Sets)
            {
                query.Append("["+TableName + "].[" + item.Key + "]=@0", item.Value);
            }
            query.Append(PocoHelper.GetConditions(q.QueryText, q.Parameters));
            int result = ((Database)_repository.Client).Execute(query);
            return result > 0;
        }
        #endregion

        #region Utilities
        private Sql ParseSql(QueryDescriptor<UserInfo> q, Sql otherCondition = null, bool isCount = false)
        {
            var columns = PocoHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM [" + TableName + "]");

            //过滤条件
            query.Append(PocoHelper.GetConditions<UserInfo>(q, otherCondition));

            //排序
            if (isCount == false)
            {
                query.Append(PocoHelper.GetOrderBy<UserInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }
        #endregion
    }
}
