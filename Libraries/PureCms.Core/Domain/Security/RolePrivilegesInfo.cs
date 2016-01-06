using System;
using PetaPoco;

namespace PureCms.Core.Domain.Security
{
    [TableName("RolePrivileges")]
    [PrimaryKey("RolePrivilegeId", autoIncrement = true)]
    public class RolePrivilegesInfo : BaseEntity
    {
        public int RolePrivilegeId { get; set; }
        public int RoleId { get; set; }
        public int PrivilegeId { get; set; }
        [Ignore]
        public new DateTime CreatedOn { get; set; }

        [ResultColumn]
        [LinkEntity(typeof(RoleInfo))]
        public string RoleName { get; set; }

        [ResultColumn]
        [LinkEntity(typeof(PrivilegeInfo),SourceFieldName = "displayname")]
        public string PrivilegeName { get; set; }
    }
}
