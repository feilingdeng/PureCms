using System.Collections.Generic;
using PetaPoco;
using PureCms.Core.Domain.Security;

namespace PureCms.Core.Domain.User
{
    [PetaPoco.TableName("users")]
    [PetaPoco.PrimaryKey("userid",autoIncrement = true)]
    public class UserInfo : BaseEntity
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
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
