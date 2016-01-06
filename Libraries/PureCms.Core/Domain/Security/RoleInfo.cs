using PetaPoco;

namespace PureCms.Core.Domain.Security
{
    [TableName("roles")]
    [PrimaryKey("roleid",autoIncrement=true)]
    public class RoleInfo : BaseEntity
    {
        public int RoleId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public int ParentRoleId { get; set; }
    }
}
