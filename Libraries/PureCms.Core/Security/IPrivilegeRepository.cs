using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Security
{
    public interface IPrivilegeRepository
    {
        int Create(PrivilegeInfo entity);

        bool Update(PrivilegeInfo entity);
        bool Update(UpdateContext<PrivilegeInfo> context);
        int MoveNode(int moveid, int targetid, int parentid, string position);

        bool DeleteById(int id);

        long Count(QueryDescriptor<PrivilegeInfo> q);
        PagedList<PrivilegeInfo> QueryPaged(QueryDescriptor<PrivilegeInfo> q);

        PrivilegeInfo FindById(int id);
        List<PrivilegeInfo> Query(QueryDescriptor<PrivilegeInfo> q);
        PrivilegeInfo Find(QueryDescriptor<PrivilegeInfo> q);
    }
}
