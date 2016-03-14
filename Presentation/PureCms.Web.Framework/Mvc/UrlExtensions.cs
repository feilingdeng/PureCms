using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace PureCms.Web.Framework
{
    public static class UrlExtensions
    {

        /// <summary>
        /// 生成链接地址
        /// </summary>
        public static string ActionLink(this UrlHelper helper, string _actionName, ViewContext _viewcontext)
        {
            RouteValueDictionary _routevalues = _viewcontext.RouteData.Values;//new RouteValueDictionary();//路由值集合
            NameValueCollection queryString = _viewcontext.RequestContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.AllKeys)
            {
                if (key != null && !_routevalues.ContainsKey(key))
                    _routevalues.Add(key, queryString[key]);
            }
            
            return helper.Action(_actionName, _routevalues);
        }
    }
}
