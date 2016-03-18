namespace PureCms.Core.Domain.Security
{
    public class UserRoleInfo : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
