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

        long Count(QueryDescriptor<RoleInfo> q);
        PagedList<RoleInfo> Query(QueryDescriptor<RoleInfo> q);

        RoleInfo GetById(int id);
        List<RoleInfo> GetAll(QueryDescriptor<RoleInfo> q);
    }
}
