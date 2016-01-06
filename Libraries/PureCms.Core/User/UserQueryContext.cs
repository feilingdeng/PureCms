using PureCms.Core.Context;
using PureCms.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.User
{
    public class UserQueryContext : QueryDescriptor<UserInfo, UserQueryContext>
    {

        /// <summary>
        /// 创建一个实例
        /// </summary>
        public UserQueryContext()
        {
        }

        public int? UserId { get; set; }
        public int? Gender { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public int? RoleId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public List<int> UserIdList { get; set; }
        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
