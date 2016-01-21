using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System.Collections.Generic;

namespace PureCms.Core.Security
{
    public interface IRoleRepository 
    {

        int Create(RoleInfo entity);

        bool Update(RoleInfo entity);

        bool DeleteById(int id);

        long Count(RoleQueryContext q);
        PagedList<RoleInfo> Query(RoleQueryContext q);

        RoleInfo GetById(int id);
        List<RoleInfo> GetAll(RoleQueryContext q);
    }
}
