using PureCms.Core.Context;
using PureCms.Core.Domain.Security;

namespace PureCms.Core.Security
{
    public class RoleQueryContext : QueryDescriptor<RoleInfo, RoleQueryContext>
    {
        public int? RoleId { get; set; }

        public string Name { get; set; }

        public bool? IsEnabled { get; set; }

        public int? ParentRoleId { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
