using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.User
{
    public interface IUserRepository
    {
        bool ExistsEmail(string email, int currentUserId = 0);
        bool ExistsUserName(string userName, int currentUserId = 0);
        bool ExistsMobile(string mobileNumber, int currentUserId = 0);
        int Create(UserInfo entity);

        bool Update(UserInfo entity);
        bool Update(UpdateContext<UserInfo> q);

        bool DeleteById(int id);
        bool DeleteById(List<int> ids);

        PagedList<UserInfo> Query(QueryDescriptor<UserInfo> q);

        UserInfo GetById(int id);

        UserInfo GetByEmail(string email);

        UserInfo GetByUserName(string userName);

        UserInfo GetByLoginName(string loginName);
        UserInfo GetByMobile(string mobileNumber);
        UserInfo GetByLoginNameAndPassword(string loginName, string password);
    }
}
