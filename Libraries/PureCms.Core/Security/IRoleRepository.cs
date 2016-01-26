using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System.Collections.Generic;

namespace PureCms.Core.Security
{
    public interface IRoleRepository 
    {

        int Create(RoleInfo entity);

        bool Update(RoleInfo entity);
        bool Update(UpdateContext<RoleInfo> context);

        bool DeleteById(int id);

        long Count(QueryDescriptor<RoleInfo> q);
        PagedList<RoleInfo> QueryPaged(QueryDescriptor<RoleInfo> q);
        List<RoleInfo> Query(QueryDescriptor<RoleInfo> q);

        RoleInfo FindById(int id);
    }
}
