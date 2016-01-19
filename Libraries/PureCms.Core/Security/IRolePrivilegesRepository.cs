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
        PagedList<RolePrivilegesInfo> Query(QueryDescriptor<RolePrivilegesInfo> q);

        RolePrivilegesInfo GetById(int id);
        List<RolePrivilegesInfo> GetAll(QueryDescriptor<RolePrivilegesInfo> q);
    }
}
