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
        int MoveNode(int moveid, int targetid, int parentid, string position);

        bool DeleteById(int id);

        long Count(QueryDescriptor<PrivilegeInfo> q);
        PagedList<PrivilegeInfo> Query(QueryDescriptor<PrivilegeInfo> q);

        PrivilegeInfo GetById(int id);
        List<PrivilegeInfo> GetAll(QueryDescriptor<PrivilegeInfo> q);
        PrivilegeInfo GetOne(QueryDescriptor<PrivilegeInfo> q);
    }
}
