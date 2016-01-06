using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System.Collections.Generic;

namespace PureCms.Core.Security
{
    public interface IRolePrivilegesRepository
    {
        int Create(RolePrivilegesInfo entity);
        bool CreateMany(List<RolePrivilegesInfo> entities);

        bool Update(RolePrivilegesInfo entity);

        bool DeleteById(int id);
        bool DeleteByRoleId(int roleId);

        long Count(RolePrivilegesQueryContext q);
        PagedList<RolePrivilegesInfo> Query(RolePrivilegesQueryContext q);

        RolePrivilegesInfo GetById(int id);
        List<RolePrivilegesInfo> GetAll(RolePrivilegesQueryContext q);
    }
}
