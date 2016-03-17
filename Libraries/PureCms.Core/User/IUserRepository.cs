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
        bool Create(SystemUserInfo entity);

        bool Update(SystemUserInfo entity);
        bool Update(UpdateContext<SystemUserInfo> q);

        bool DeleteById(Guid id);
        bool DeleteById(List<Guid> ids);

        PagedList<SystemUserInfo> QueryPaged(QueryDescriptor<SystemUserInfo> q);
        List<SystemUserInfo> Query(QueryDescriptor<SystemUserInfo> q);

        SystemUserInfo FindById(Guid id);

        SystemUserInfo FindByEmail(string email);

        SystemUserInfo FindByUserName(string userName);

        SystemUserInfo FindByLoginName(string loginName);
        SystemUserInfo FindByMobile(string mobileNumber);
        SystemUserInfo FindByLoginNameAndPassword(string loginName, string password);
    }
}
