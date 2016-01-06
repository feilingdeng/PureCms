using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Security
{
    public class RolePrivilegesQueryContext : QueryDescriptor<RolePrivilegesInfo, RolePrivilegesQueryContext>
    {
        public int? RolePrivilegeId { get; set; }
        public int? RoleId { get; set; }
        public int? PrivilegeId { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
