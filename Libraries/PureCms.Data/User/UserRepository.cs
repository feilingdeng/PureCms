using PureCms.Core.User;
using System.Collections.Generic;
using PureCms.Core.Domain.User;
using PureCms.Core.Context;
using System;

namespace PureCms.Data.User
{
    public class UserRepository : IUserRepository
    {
        private static readonly DataRepository<SystemUserInfo> _repository = new DataRepository<SystemUserInfo>();

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
            QueryDescriptor<SystemUserInfo> q = new QueryDescriptor<SystemUserInfo>();
            FilterContainer<SystemUserInfo> filter = new FilterContainer<SystemUserInfo>();
            filter.And(w => w.EmailAddress == email);
            if (currentUserId.HasValue)
            {
                filter.And(w => w.SystemUserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }
        public bool ExistsUserName(string userName, Guid? currentUserId)
        {
            QueryDescriptor<SystemUserInfo> q = new QueryDescriptor<SystemUserInfo>();
            FilterContainer<SystemUserInfo> filter = new FilterContainer<SystemUserInfo>();
            filter.And(w => w.Name == userName);
            if (currentUserId.HasValue)
            {
                filter.And(w => w.SystemUserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }
        public bool ExistsMobile(string mobileNumber, Guid? currentUserId)
        {
            QueryDescriptor<SystemUserInfo> q = new QueryDescriptor<SystemUserInfo>();
            FilterContainer<SystemUserInfo> filter = new FilterContainer<SystemUserInfo>();
            filter.And(w => w.MobileNumber == mobileNumber);
            if (currentUserId.HasValue)
            {
                filter.And(w=>w.SystemUserId != currentUserId.Value);
            }
            q.Where(filter);
            return _repository.Exists(q);
        }

        public bool Create(SystemUserInfo entity)
        {
            return _repository.CreateObject(entity);
        }

        public bool Update(SystemUserInfo entity)
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

        public PagedList<SystemUserInfo> QueryPaged(QueryDescriptor<SystemUserInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<SystemUserInfo> Query(QueryDescriptor<SystemUserInfo> q)
        {
            return _repository.Query(q);
        }

        public SystemUserInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public SystemUserInfo FindByEmail(string email)
        {
            return _repository.Find(w => w.EmailAddress == email);
        }

        public SystemUserInfo FindByUserName(string userName)
        {
            return _repository.Find(w => w.Name == userName);
        }

        public SystemUserInfo FindByMobile(string mobileNumber)
        {
            return _repository.Find(w => w.MobileNumber == mobileNumber);
        }

        public SystemUserInfo FindByLoginName(string loginName)
        {
            return _repository.Find(w => w.LoginName == loginName);
        }

        public SystemUserInfo FindByLoginNameAndPassword(string loginName, string password)
        {
            return _repository.Find(w => w.LoginName == loginName && w.Password == password);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Update(UpdateContext<SystemUserInfo> context)
        {
            return _repository.Update(context);
        }
        #endregion
    }
}
