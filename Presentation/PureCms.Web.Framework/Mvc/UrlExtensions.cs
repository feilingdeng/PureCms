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
        public static string ActionUrl(this UrlHelper helper, string _actionName, ViewContext _viewcontext, object _routeValues = null)
        {
            RouteValueDictionary _routed = _viewcontext.RouteData.Values;//new RouteValueDictionary();//路由值集合
            NameValueCollection queryString = _viewcontext.RequestContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.AllKeys)
            {
                if (key != null && !_routed.ContainsKey(key))
                    _routed.Add(key, queryString[key]);
            }
            //if (_routeValues != null)
            //{
            //    RouteValueDictionary _routec = _routeValues as RouteValueDictionary;
            //    foreach (var item in _routec)
            //    {
            //        if (!_routed.ContainsKey(item.Key))
            //            _routed.Add(item.Key, item.Value);
            //        else
            //            _routed[item.Key] = item.Value;
            //    }
            //}

            return helper.Action(_actionName, _routed);
        }
    }
}
