using System.Collections.Generic;
using PetaPoco;
using PureCms.Core.Domain.Security;
using System;

namespace PureCms.Core.Domain.User
{
    [PetaPoco.TableName("users")]
    [PetaPoco.PrimaryKey("userid",autoIncrement = false)]
    public class UserInfo : BaseEntity
    {
        public Guid UserId { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }
        public string Avator { get; set; }
        public string Salt { get; set; }
        public int Gender { get; set; }
        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [ResultColumn]
        public List<RolePrivilegesInfo> Privileges { get; set; }
    }
}
