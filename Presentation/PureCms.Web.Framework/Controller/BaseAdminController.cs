using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using PureCms.Core;
using PureCms.Services.Logging;
using PureCms.Core.Domain.Logging;
using PureCms.Utilities;
using PureCms.Services.Security;
using PureCms.Services.Configuration;
using PureCms.Web.Framework.Mvc;
//using PureCms.Services;

namespace PureCms.Web.Framework
{
    [AuthorizeFilter]
    /// <summary>
    /// 基础控制器类
    /// </summary>
    public class BaseAdminController : Controller
    {
        public FormsAuthenticationService _authenticationService;
        public static LogService _logService = new LogService();
        public static SettingService _settingService = new SettingService();
        public static SecuritySetting _securitySetting = _settingService.GetSecuritySetting();
        
        //工作上下文
        public AdminWorkContext WorkContext = new AdminWorkContext();

        public bool IsAjaxRequest
        {
            get
            {
                return WorkContext.IsHttpAjax;
            }
        }

        public PlatformSetting PlatformSettings
        {
            get
            {
                return _settingService.GetPlatformSetting();
            }
        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.ValidateRequest = false;
            _authenticationService = new FormsAuthenticationService(requestContext.HttpContext);

            WorkContext.IsHttpAjax = WebHelper.IsAjaxRequest();
            WorkContext.IP = WebHelper.GetCurrentIpAddress();
            WorkContext.Url = WebHelper.GetThisPageUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();

            WorkContext.CurrentUser = _authenticationService.GetAuthenticatedUser();

            //设置当前控制器类名
            WorkContext.Controller = RouteData.Values["controller"].ToString().ToLower();
            //设置当前动作方法名
            WorkContext.Action = RouteData.Values["action"].ToString().ToLower();
            WorkContext.PageKey = string.Format("/{0}/{1}", WorkContext.Controller, WorkContext.Action);

            WorkContext.ImageCDN = PlatformSettings.ImageCDN;
            WorkContext.CSSCDN = PlatformSettings.CSSCDN;
            WorkContext.ScriptCDN = PlatformSettings.ScriptCDN;


        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;


            //NavPath = new PrivilegeService().GetTreePath(WorkContext.Controller, WorkContext.Action);

            AdminActionLogging(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //不能应用在子方法上
            if (filterContext.IsChildAction)
                return;

            AdminActionLogging(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (IsAjaxRequest)
                filterContext.Result = AjaxResult("error", "系统错误,请联系管理员");
            else
                filterContext.Result = new ViewResult() { ViewName = "error" };


            AdminActionLogging(filterContext);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected string GetRouteString(string key, string defaultValue)
        {
            object value = RouteData.Values[key];
            if (value != null)
                return value.ToString();
            else
                return defaultValue;
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetRouteString(string key)
        {
            return GetRouteString(key, "");
        }

        /// <summary>
        /// 获得路由中的值或URL参数的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected string GetRouteOrQueryString(string key)
        {
            return GetRouteString(key, null) ?? Request.QueryString[key];
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        protected int GetRouteInt(string key, int defaultValue)
        {
            return 0;//TypeHelper.ObjectToInt(RouteData.Values[key], defaultValue);
        }

        /// <summary>
        /// 获得路由中的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected int GetRouteInt(string key)
        {
            return GetRouteInt(key, 0);
        }

        /// <summary>
        /// 获得路由中的值或URL参数的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        protected int GetRouteOrQueryInt(string key)
        {
            return GetRouteInt(key) > 0 ? GetRouteInt(key) : (!string.IsNullOrEmpty(Request.QueryString[key])?int.Parse(Request.QueryString[key]):-1);
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string message)
        {
            return View("prompt", new PromptModel(message));
        }

        /// <summary>
        /// 提示信息视图
        /// </summary>
        /// <param name="backUrl">返回地址</param>
        /// <param name="message">提示信息</param>
        /// <returns></returns>
        protected ViewResult PromptView(string backUrl, string message)
        {
            return View("prompt", new PromptModel(backUrl, message));
        }

        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(bool isSuccess, object content)
        {
            return new JsonResult() { Data = new JsonResultObject() { StatusName = isSuccess?"success":"error", Content = content.SerializeToJson()  }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(bool isSuccess, string content)
        {
            return new JsonResult() { Data = new JsonResultObject() { StatusName = isSuccess?"success":"error", Content = content  }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="statusName">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string statusName, object content)
        {
            return new JsonResult() { Data = new JsonResultObject() { StatusName = statusName, Content = content.SerializeToJson() }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// ajax请求结果
        /// </summary>
        /// <param name="statusName">状态</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        protected ActionResult AjaxResult(string statusName, string content)
        {
            return new JsonResult() { Data = new JsonResultObject() { StatusName = statusName, Content = content }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public void AdminActionLogging(ControllerContext context)
        {
            //日志记录
            var controllerName = WorkContext.Controller + "." + WorkContext.Action;
            var actionName = context.RouteData.Values["action"].ToString();
            var description = controllerName + "." + actionName;
            var title = "进入菜单";
            var isException = context is ExceptionContext;
            if (isException)
            {
                var exContext = (ExceptionContext)context;
                title = exContext.Exception.Message;
                description = exContext.Exception.StackTrace;
            }
            _logService.Create(new LogInfo()
            {
                Title = title
                ,
                Url = WorkContext.Url
                ,
                UrlReferrer = WorkContext.UrlReferrer
                ,
                UserId = WorkContext.CurrentUser == null ? -1 :WorkContext.CurrentUser.UserId
                ,
                StatusCode = context.HttpContext.Response.StatusCode
                ,
                Description = description
                ,
                ClientIP = WorkContext.IP
                ,
                CreatedOn = DateTime.Now
            });
        }
    }
}
