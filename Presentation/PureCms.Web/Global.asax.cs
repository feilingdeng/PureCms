using PureCms.Core.Domain.Logging;
using PureCms.Core.Plugins;
using PureCms.Utilities;
using PureCms.Web.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PureCms.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            //将默认视图引擎替换为ThemeRazorViewEngine引擎
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeRazorViewEngine());

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        /// <summary>
        /// 应用错误处理
        /// </summary>
        protected void Application_Error()
        {
            if (Context.IsCustomErrorEnabled)
            {
                //当前错误
                Exception exception = Server.GetLastError();

                #region 记录错误日志

                var httpException = exception as HttpException ?? new HttpException(500, "服务器内部错误", exception);

                LogInfo entity = new LogInfo()
                {
                    Id = DateTime.Now.ToString("ddHHmmssfff"),
                    CreatedOn = DateTime.Now,
                    Description = exception.StackTrace,
                    Title = exception.Message,
                    StatusCode = httpException.GetHttpCode(),
                    Url = WebHelper.GetThisPageUrl(),
                    UrlReferrer = WebHelper.GetUrlReferrer(),
                    ClientIP = WebHelper.GetCurrentIpAddress()
                };


                #endregion

                #region 处理错误页面

                Response.Clear();
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("fromAppErrorEvent", true);

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "Error404");
                        break;

                    case 500:
                        routeData.Values.Add("action", "Error500");
                        break;

                    default:
                        routeData.Values.Add("action", "GeneralError");
                        routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                        break;
                }

                Server.ClearError();

                #endregion

                #region 跳转错误页面

                if (!WebHelper.IsAjaxRequest())
                {
                    //跳转错误处理action参数添加
                    //routeData.Values.Add("url", uri);

                    IController controller = new ErrorController();
                    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                }

                #endregion
            }
        }
        //protected void Application_BeginRequest(Object source,
        // EventArgs e)
        //{
        //    HttpApplication application = (HttpApplication)source;
        //    HttpContext context = application.Context;
        //    string filePath = context.Request.FilePath;
        //    string fileExtension =
        //        VirtualPathUtility.GetExtension(filePath);
        //    if (fileExtension.Equals(".aspx"))
        //    {
        //        context.Response.Write("<h1><font color=red>" +
        //            "HelloWorldModule: Beginning of Request" +
        //            "</font></h1><hr>");
        //    }
        //}
    }
}