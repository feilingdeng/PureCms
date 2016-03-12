using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;

namespace PureCms.Services.Security
{
    public class CurrentUser
    {
        public const string SESSION_KEY = "$PureCms.CurrentUser$";
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }

        public bool IsSuperAdmin { get; set; }

        public List<RolePrivilegesInfo> Privileges { get; set; }
    }
}
