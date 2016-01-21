using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Services.Security
{
    public class CurrentUser
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }

        public bool IsSuperAdmin { get; set; }

        public List<RolePrivilegesInfo> Privileges { get; set; }
    }
}
