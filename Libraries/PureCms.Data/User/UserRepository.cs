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
            UserQueryContext q = new UserQueryContext
            {
                EmailAddress = email
            };
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = ParseContext(q, otherCondition);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        public bool ExistsUserName(string userName, int currentUserId = 0)
        {
            UserQueryContext q = new UserQueryContext
            {
                UserName = userName
            };
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = ParseContext(q, otherCondition);
            var result = _repository.ExistsAsync(ctx);
            return result.Result;
        }
        public bool ExistsMobile(string mobileNumber, int currentUserId = 0)
        {
            UserQueryContext q = new UserQueryContext
            {
                MobileNumber = mobileNumber
            };
            var otherCondition = PetaPoco.Sql.Builder;
            if (currentUserId > 0)
            {
                otherCondition.Append("UserId != @0", currentUserId);
            }
            ExecuteContext<UserInfo> ctx = ParseContext(q, otherCondition);
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

        public PagedList<UserInfo> Query(UserQueryContext q)
        {
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<UserInfo> list = new PagedList<UserInfo>() { 
                    CurrentPage = pageDatas.CurrentPage
                    ,ItemsPerPage = pageDatas.ItemsPerPage
                    ,TotalItems = pageDatas.TotalItems
                    ,TotalPages = pageDatas.TotalPages
                    ,Items = pageDatas.Items
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
            UserQueryContext q = new UserQueryContext
            {
                EmailAddress = email
            };
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByUserName(string userName)
        {
            UserQueryContext q = new UserQueryContext
            {
                UserName = userName
            };
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByMobile(string mobileNumber)
        {
            UserQueryContext q = new UserQueryContext
            {
                MobileNumber = mobileNumber
            };
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByLoginName(string loginName)
        {
            UserQueryContext q = new UserQueryContext
            {
                LoginName = loginName
            };
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }

        public UserInfo GetByLoginNameAndPassword(string loginName, string password)
        {
            UserQueryContext q = new UserQueryContext
            {
                LoginName = loginName
                ,Password = password
            };
            ExecuteContext<UserInfo> ctx = ParseContext(q);
            var result = _repository.GetSingleAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="q">过滤条件</param>
        /// <returns></returns>
        public bool Update(List<KeyValuePair<string, object>> sets, UserQueryContext q)
        {
            Guard.ArgumentNotNullOrEmpty<KeyValuePair<string, object>>(sets, "sets");

            Sql query = PetaPoco.Sql.Builder.Append("UPDATE " + TableName + " SET ");
            string optName = string.Empty;
            foreach (var item in sets)
            {
                query.Append(TableName + "." + item.Key + "=@0", item.Value);
            }
            query.Append(ParseWhereSql(q));
            int result = ((Database)_repository.Client).Execute(query);
            return result > 0;
        }
        #endregion

        #region Utilities
        private Sql ParseSql(UserQueryContext q, Sql otherCondition = null,bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM Users");
            //过滤条件
            query.Append(ParseWhereSql(q,otherCondition));
            
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<UserInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }


        /// <summary>
        /// 根据上下文生成过滤条件语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <returns></returns>
        private Sql ParseWhereSql(UserQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.EmailAddress.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.EmailAddress=@0", optName, TableName), q.EmailAddress);
                optName = " AND ";
            }
            if (q.UserName.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.UserName=@0", optName, TableName), q.UserName);
                optName = " AND ";
            }
            if (q.LoginName.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.LoginName=@0", optName, TableName), q.LoginName);
                optName = " AND ";
            }
            if (q.Gender.HasValue)
            {
                filter.Append(string.Format("{0} {1}.Gender=@0", optName, TableName), q.Gender.Value);
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

        private ExecuteContext<UserInfo> ParseContext(UserQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<UserInfo> ctx = new ExecuteContext<UserInfo>()
            {
                ExecuteContainer = ParseSql(q, otherCondition,isCount)
                ,
                PagingInfo = q.PagingDescriptor
                //,
                //SortingInfo = q.SortingInfo
                ,
                TopCount = q.TopCount
            };

            return ctx;
        } 
        #endregion
    }
}
