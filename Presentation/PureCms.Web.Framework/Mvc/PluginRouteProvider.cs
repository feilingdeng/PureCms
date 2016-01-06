using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PureCms.Services.Plugins;
using PureCms.Core.Plugins;

namespace PureCms.Web.Framework.Mvc
{
    public class PluginRouteProvider : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var virtualPath = httpContext.Request.AppRelativeCurrentExecutionFilePath + httpContext.Request.PathInfo;//获取相对路径

            //virtualPath = virtualPath.Substring(2).Trim('/');//此时URL会是～/ca-categoryname，截取后面的ca-categoryname

            if (!virtualPath.Contains("plugin-"))
                return null;

            string[] routeVars = virtualPath.Split('-');
            string pluginSystemName = routeVars[1];
            string pluginController = routeVars[2];
            string pluginAction = routeVars[3];
            PluginDescriptor descriptor = PluginService.GetPluginBySystemName(pluginSystemName);
            if(null == descriptor)
            {
                return null;
            }
            string pluginNamespace = descriptor.Namespace + ".Controllers";
            string pluginFolderName = descriptor.FolderName;

            //至此可以肯定是我们要处理的URL了
            var routeData = new RouteData(this, new MvcRouteHandler());//声明一个RouteData，添加相应的路由值
            routeData.DataTokens.Remove("Namespaces");
            routeData.DataTokens["Namespaces"] = new string[1] { pluginNamespace };
            routeData.DataTokens["area"] = "plugin";
            routeData.Values.Add("pluginsystemname", pluginSystemName);
            routeData.Values.Add("plugincontroller", pluginController);
            routeData.Values.Add("pluginaction", pluginAction);
            routeData.Values.Add("pluginfoldername", pluginFolderName);
            routeData.Values.Add("controller", pluginController);
            routeData.Values.Add("action", pluginAction);

            return routeData;//返回这个路由值将调用CategoryController.ShowCategory(category.CategoeyID)方法。匹配终止
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //请求不是CategoryController发起的，不是我们要处理的请求，返回null
            if (!values.ContainsKey("pluginsystemname"))
                return null;


            var path = requestContext.HttpContext.Request.Path;
            return new VirtualPathData(this, path.ToLowerInvariant());
        }
    }
}
