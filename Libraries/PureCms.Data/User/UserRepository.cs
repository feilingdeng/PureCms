using PureCms.Core.User;
using System.Collections.Generic;
using PureCms.Core.Domain.User;
using PureCms.Core.Context;
using System;

namespace PureCms.Data.User
{
    public class UserRepository : IUserRepository
    {
        private static readonly DataRepository<UserInfo> _repository = new DataRepository<UserInfo>();

        /// <summary>
        /// 实体元数据
        /// </summary>
        private PetaPoco.Database.PocoData MetaData
        {
            get
            {
                return _repository.MetaData;
            }
        }

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
        public bool ExistsEmail(string email, Guid? currentUserId)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            FilterContainer<UserInfo> filter = new FilterContainer<UserInfo>();
            filter.And(w => w.EmailAddress == email);
            if (currentUserId.HasValue)
            {
                filter.And(w => w.UserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }
        public bool ExistsUserName(string userName, Guid? currentUserId)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            FilterContainer<UserInfo> filter = new FilterContainer<UserInfo>();
            filter.And(w => w.Name == userName);
            if (currentUserId.HasValue)
            {
                filter.And(w => w.UserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }
        public bool ExistsMobile(string mobileNumber, Guid? currentUserId)
        {
            QueryDescriptor<UserInfo> q = new QueryDescriptor<UserInfo>();
            FilterContainer<UserInfo> filter = new FilterContainer<UserInfo>();
            filter.And(w => w.MobileNumber == mobileNumber);
            if (currentUserId.HasValue)
            {
                filter.And(w=>w.UserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }

        public bool Create(UserInfo entity)
        {
            return _repository.CreateObject(entity);
        }

        public bool Update(UserInfo entity)
        {
            return _repository.Update(entity);
        }

        public bool DeleteById(Guid id)
        {
            return _repository.Delete(id);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteById(List<Guid> ids)
        {
            return _repository.Delete(ids);
        }

        public PagedList<UserInfo> QueryPaged(QueryDescriptor<UserInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<UserInfo> Query(QueryDescriptor<UserInfo> q)
        {
            return _repository.Query(q);
        }

        public UserInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public UserInfo FindByEmail(string email)
        {
            return _repository.Find(w => w.EmailAddress == email);
        }

        public UserInfo FindByUserName(string userName)
        {
            return _repository.Find(w => w.Name == userName);
        }

        public UserInfo FindByMobile(string mobileNumber)
        {
            return _repository.Find(w => w.MobileNumber == mobileNumber);
        }

        public UserInfo FindByLoginName(string loginName)
        {
            return _repository.Find(w => w.LoginName == loginName);
        }

        public UserInfo FindByLoginNameAndPassword(string loginName, string password)
        {
            return _repository.Find(w => w.LoginName == loginName && w.Password == password);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Update(UpdateContext<UserInfo> context)
        {
            return _repository.Update(context);
        }
        #endregion
    }
}
