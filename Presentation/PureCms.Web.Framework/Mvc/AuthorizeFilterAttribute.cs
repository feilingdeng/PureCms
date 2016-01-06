using PureCms.Services.Security;
using PureCms.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PureCms.Web.Framework.Mvc
{
    /// <summary>
    /// 权限拦截及验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public FormsAuthenticationService _authenticationService;
        public CurrentUser _currentUser;
        public static PermissionService _permissionService = new PermissionService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            var path = filterContext.HttpContext.Request.Path.ToLower();
            if (path == "/" || path == "/admin/home/signin".ToLower() || path == "/account/signin".ToLower())
                return;//忽略对Login登录页的权限判定
            
            _authenticationService = new FormsAuthenticationService(filterContext.HttpContext);
            //未登录时转到登录页
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var result = new RedirectResult(_authenticationService.LoginUrl + "?returnurl=" + WebHelper.GetUrlReferrer());
                filterContext.Result = result;
                return;
            }
            _currentUser = _authenticationService.GetAuthenticatedUser();//获取当前用户信息
            if(_currentUser == null)
            {
                var result = new RedirectResult(_authenticationService.LoginUrl + "?returnurl=" + WebHelper.GetUrlReferrer());
                filterContext.Result = result;
                return;
            }
            else if (_currentUser.IsSuperAdmin)//超级管理员不验证权限
            {
                return;
            }
            else
            {
                //object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ViewPageAttribute), true);
                var isViewPage = !filterContext.IsChildAction;//当前Action请求是否为具体的功能页

                if (this.Valid(filterContext, isViewPage) == false)//根据验证判断进行处理
                {
                    //注：如果未登录直接在URL输入功能权限地址提示不是很友好；如果登录后输入未维护的功能权限地址，那么也可以访问，这个可能会有安全问题
                    if (isViewPage == true)
                    {
                        var result = new RedirectResult("/admin/home/Unauthorized");
                        filterContext.Result = result;// new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页
                    }
                    else
                    {
                        filterContext.Result = new JsonResult() { Data = new JsonResultObject() { StatusName = "Unauthorized", Content = "没有权限" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
        }
        //权限判断业务逻辑
        protected virtual bool Valid(ActionExecutingContext filterContext, bool isViewPage)
        {
            if (filterContext.HttpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            var controllerName = filterContext.RouteData.Values["controller"].ToString();
            var actionName = filterContext.RouteData.Values["action"].ToString();
            if (isViewPage && (controllerName.ToLower() == "main" && actionName.ToLower() == "masterpage"))//如果是MasterPage页
            {
                    return true;
            }
            //功能页
            var p = _permissionService.HasPermission(_currentUser, controllerName, actionName);
            return p;
        }
    }
}
