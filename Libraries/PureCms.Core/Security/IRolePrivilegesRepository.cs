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

        long Count(QueryDescriptor<RolePrivilegesInfo> q);
        PagedList<RolePrivilegesInfo> QueryPaged(QueryDescriptor<RolePrivilegesInfo> q);

        RolePrivilegesInfo FindById(int id);
        List<RolePrivilegesInfo> Query(QueryDescriptor<RolePrivilegesInfo> q);
    }
}
