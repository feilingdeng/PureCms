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
        public int Create(UserInfo entity)
        {
            int id = _repository.Create(entity);
            return id;
        }
        public bool Update(UserInfo entity)
        {
            return _repository.Update(entity);
        }

        public UserInfo GetUserByUserName(string userName)
        {
            return _repository.GetByUserName(userName);
        }
        public UserInfo GetUserByEmail(string email)
        {
            return _repository.GetByEmail(email);
        }
        public UserInfo GetUserByMobile(string mobileNumber)
        {
            return _repository.GetByMobile(mobileNumber);
        }
        public UserInfo GetUserByLoginName(string loginName)
        {
            return _repository.GetByLoginName(loginName);
        }
        public UserInfo GetUserByLoginNameAndPassword(string loginName, string password)
        {
            return _repository.GetByLoginNameAndPassword(loginName, password);
        }
        public bool IsValidePassword(string inputPassword, string salt, string userPassword)
        {
            return SecurityHelper.MD5(inputPassword + salt).IsCaseInsensitiveEqual(userPassword);
        }

        public UserInfo GetById(int id)
        {
            return _repository.GetById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<int> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<UserInfo> Query(Func<UserQueryContext, UserQueryContext> container)
        {
            UserQueryContext q = container(new UserQueryContext());

            return _repository.Query(q);
        }
        public bool Update(Func<UpdateContext<UserInfo>, UpdateContext<UserInfo>> context)
        {
            var ctx = context(new UpdateContext<UserInfo>());
            return _repository.Update(ctx);
        }
    }
}
