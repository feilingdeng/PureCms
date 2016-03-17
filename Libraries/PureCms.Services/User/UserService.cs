using PureCms.Core.Domain.User;
using System;
using System.Collections.Generic;
using PureCms.Data.User;
using PureCms.Core.User;
using PureCms.Core.Utilities;
using PureCms.Core.Context;

namespace PureCms.Services.User
{
    public class UserService
    {
        IUserRepository _repository = new UserRepository();
        public bool Create(SystemUserInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(SystemUserInfo entity)
        {
            return _repository.Update(entity);
        }

        public SystemUserInfo GetUserByUserName(string userName)
        {
            return _repository.FindByUserName(userName);
        }
        public SystemUserInfo GetUserByEmail(string email)
        {
            return _repository.FindByEmail(email);
        }
        public SystemUserInfo GetUserByMobile(string mobileNumber)
        {
            return _repository.FindByMobile(mobileNumber);
        }
        public SystemUserInfo GetUserByLoginName(string loginName)
        {
            return _repository.FindByLoginName(loginName);
        }
        public SystemUserInfo GetUserByLoginNameAndPassword(string loginName, string password)
        {
            return _repository.FindByLoginNameAndPassword(loginName, password);
        }
        public bool IsValidePassword(string inputPassword, string salt, string userPassword)
        {
            return SecurityHelper.MD5(inputPassword + salt).IsCaseInsensitiveEqual(userPassword);
        }

        public SystemUserInfo GetById(Guid id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<Guid> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<SystemUserInfo> Query(Func<QueryDescriptor<SystemUserInfo>, QueryDescriptor<SystemUserInfo>> container)
        {
            QueryDescriptor<SystemUserInfo> q = container(new QueryDescriptor<SystemUserInfo>());

            return _repository.QueryPaged(q);
        }
        public bool Update(Func<UpdateContext<SystemUserInfo>, UpdateContext<SystemUserInfo>> context)
        {
            var ctx = context(new UpdateContext<SystemUserInfo>());
            return _repository.Update(ctx);
        }
    }
}
