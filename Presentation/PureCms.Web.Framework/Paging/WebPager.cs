using System.Collections.Specialized;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace PureCms.Web.Framework
{
    /// <summary>
    /// 分页类
    /// </summary>
    public class WebPager : Pager
    {
        private string _routename = null;//路由名
        private string _pageparamname = "page";//页参数名
        private ViewContext _viewcontext = null;//视图上下文
        private RouteValueDictionary _routevalues = new RouteValueDictionary();//路由值集合

        bool IsAlwaysShowPager = false;
        public WebPager(PagedModel pageModel, ViewContext viewContext)
            : base(pageModel)
        {
            _viewcontext = viewContext;

            NameValueCollection queryString = _viewcontext.RequestContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.AllKeys)
            {
                if (key != null)
                    _routevalues.Add(key, queryString[key]);
            }

            if (!_routevalues.ContainsKey(_pageparamname))
                _routevalues.Add(_pageparamname, 1);
        }

        public WebPager(PagedModel pageModel, ViewContext viewContext, bool isAlwaysShowPager)
            : base(pageModel)
        {
            _viewcontext = viewContext;

            this.IsAlwaysShowPager = isAlwaysShowPager;
            NameValueCollection queryString = _viewcontext.RequestContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.AllKeys)
            {
                if (key != null)
                    _routevalues.Add(key, queryString[key]);
            }

            if (!_routevalues.ContainsKey(_pageparamname))
                _routevalues.Add(_pageparamname, 1);
        }

        /// <summary>
        /// 设置路由名
        /// </summary>
        /// <param name="name">路由名称</param>
        /// <returns></returns>
        public Pager RouteName(string name)
        {
            _routename = name;
            return this;
        }

        /// <summary>
        /// 设置页参数名
        /// </summary>
        /// <param name="name">页参数名称</param>
        /// <returns></returns>
        public Pager PageParamName(string name)
        {
            _pageparamname = name;
            return this;
        }

        /// <summary>
        /// 设置路由值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Pager RouteValues(string key, string value)
        {
            if (_routevalues.ContainsKey(key))
                _routevalues[key] = value;
            else
                _routevalues.Add(key, value);
            return this;
        }

        public sealed override string ToString()
        {
            if (_pagemodel.TotalCount == 0 || _pagemodel.TotalCount <= _pagemodel.PageSize)
            {
                if (!IsAlwaysShowPager)
                {
                    return null;
                }
            }

            StringBuilder html = new StringBuilder("<div class=\"row\"><div class=\"col-sm-4\">");

            if (_showsummary)
            {
                html.AppendFormat("当前{0}/{1}页, 共{2}行记录", _pagemodel.PageNumber, _pagemodel.TotalPages, _pagemodel.TotalCount);
            }
            html.Append("</div>");
            html.Append("<div class=\"col-sm-8\">");
            html.Append("<ul class=\"pagination\">");
            if (_showfirst)
            {
                if (_pagemodel.IsFirstPage)
                    html.Append("<li><a href=\"javascript:void(0)\" class=\"disabled\">&laquo;</a></li>");
                else
                    html.AppendFormat("<li><a href=\"{0}\">&laquo;</a></li>", CreateUrl(1));
            }

            if (_showpre)
            {
                if (_pagemodel.HasPrePage)
                    html.AppendFormat("<li><a href=\"{0}\">&lsaquo;</a></li>", CreateUrl(_pagemodel.PrePageNumber));
                else
                    html.Append("<li><a href=\"javascript:void(0)\" class=\"disabled\">&lsaquo;</a></li>");
            }

            if (_showitems)
            {
                int startPageNumber = GetStartPageNumber();
                int endPageNumber = GetEndPageNumber();
                    html.AppendFormat(GetPageItems());
                //for (int i = startPageNumber; i <= endPageNumber; i++)
                //{
                //    //if (_pagemodel.PageNumber == i)
                //    //    html.AppendFormat("<li class=\"active\"><a href=\"javascript:void(0)\">{0}</a></li>", i);
                //    //else
                //    //    html.AppendFormat("<li><a href=\"{1}\">{0}</a></li>", i, CreateUrl(i));
                //}
            }

            if (_shownext)
            {
                if (_pagemodel.HasNextPage)
                    html.AppendFormat("<li><a href=\"{0}\">&rsaquo;</a></li>", CreateUrl(_pagemodel.NextPageNumber));
                else
                    html.Append("<li><a href=\"javascript:void(0)\" class=\"disabled\">&rsaquo;</a></li>");
            }

            if (_showlast)
            {
                if (_pagemodel.IsLastPage)
                    html.Append("<li><a href=\"javascript:void(0)\" class=\"disabled\">&raquo;</a></li>");
                else
                    html.AppendFormat("<li><a href=\"{0}\">&raquo;</a></li>", CreateUrl(_pagemodel.TotalPages));
            }

            html.Append("<ul></div></div>");
            return html.ToString();
        }

        private string GetPageItems()
        {
            StringBuilder pageStr = new StringBuilder();
            int centSize = 9; //中间页码数量

            if (_pagemodel.PageNumber == 1)
            {
                pageStr.Append("<li class=\"active\"><a href=\"javascript:void(0)\">1</a></li>");
            }
            else if (_pagemodel.PageNumber == _pagemodel.TotalPages)
            {
                pageStr.Append("<li class=\"active\"><a href=\"javascript:void(0)\">" + _pagemodel.TotalPages.ToString() + "</a></li>");
            }
            int firstNum = _pagemodel.PageNumber - (centSize / 2); //中间开始的页码
            if (_pagemodel.PageNumber < centSize)
                firstNum = 2;
            int lastNum = _pagemodel.PageNumber + centSize - ((centSize / 2) + 1); //中间结束的页码
            if (lastNum >= _pagemodel.TotalPages)
                lastNum = _pagemodel.TotalPages - 1;

            if (_pagemodel.PageNumber >= centSize)
            {
                pageStr.Append("<li><a href=\"" + CreateUrl(firstNum + 1) + "\">...</a></li>\n");
            }
            for (int i = firstNum; i <= lastNum; i++)
            {
                if (i == _pagemodel.PageNumber)
                {
                    pageStr.Append("<li class=\"active\"><a href=\"javascript:void(0)\">" + i + "</a></span>");
                }
                else
                {
                    pageStr.Append("<li><a href=\"" + CreateUrl(i) + "\">" + i + "</a></li>");
                }
            }
            if (_pagemodel.TotalPages - _pagemodel.PageNumber > centSize - ((centSize / 2)))
            {
                pageStr.Append("<li><a href=\"" + CreateUrl(lastNum + 1) + "\">...</a></li>");
            }
            //pageStr.Append(_pagemodel.TotalPages == 1 ? lastBtn : lastStr + lastBtn);
            return pageStr.ToString();
        }

        /// <summary>
        /// 生成链接地址
        /// </summary>
        /// <param name="pageNumber">页数</param>
        /// <returns></returns>
        private string CreateUrl(int pageNumber)
        {
            _routevalues[_pageparamname] = pageNumber;
            return UrlHelper.GenerateUrl(_routename, null, null, _routevalues, RouteTable.Routes, _viewcontext.RequestContext, true);
        }
    }
}
