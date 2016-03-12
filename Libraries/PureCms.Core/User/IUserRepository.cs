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
        bool ExistsEmail(string email, Guid? currentUserId);
        bool ExistsUserName(string userName, Guid? currentUserId);
        bool ExistsMobile(string mobileNumber, Guid? currentUserId);
        bool Create(UserInfo entity);

        bool Update(UserInfo entity);
        bool Update(UpdateContext<UserInfo> q);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        PagedList<UserInfo> QueryPaged(QueryDescriptor<UserInfo> q);
        List<UserInfo> Query(QueryDescriptor<UserInfo> q);

        UserInfo FindById(Guid id);

        UserInfo FindByEmail(string email);

        UserInfo FindByUserName(string userName);

        UserInfo FindByLoginName(string loginName);
        UserInfo FindByMobile(string mobileNumber);
        UserInfo FindByLoginNameAndPassword(string loginName, string password);
    }
}
