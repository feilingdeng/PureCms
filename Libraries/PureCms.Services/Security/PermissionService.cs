using System.Collections.Generic;
using PureCms.Core.Domain.Security;
using PureCms.Core.Domain.User;
using PureCms.Core;

namespace PureCms.Services.Security
{
    public class PermissionService
    {
        private static PrivilegeService _privilegeService = new PrivilegeService();
        public PermissionService() { }
        public bool HasPermission(CurrentUser currentUser, string url)
        {
            return true;
        }
        /// <summary>
        /// 权限判断
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="ignoreNull">是否忽略不存在的权限项</param>
        /// <returns></returns>
        public bool HasPermission(CurrentUser currentUser, string className, string methodName, bool ignoreNull = true)
        {
            //获取权限项
            var p = _privilegeService.GetOne(q=>q.Where(w=>w.ClassName, className).Where(w=>w.MethodName, methodName));
            if (ignoreNull && null == p)
            {
                return true;
            }
            if (currentUser.Privileges.IsNotNullOrEmpty() && currentUser.Privileges.Find(n => n.PrivilegeId == p.PrivilegeId) != null)
            {
                return true;
            }
            return false;
        }
        public bool HasPermission(CurrentUser currentUser, int privilegeId)
        {
            return true;
        }
    }
}
