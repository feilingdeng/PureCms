using System;
using System.IO;
using System.Web.Mvc;

namespace PureCms.Web.Framework
{
    public static class ViewExtensions
    {
        /// <summary>
        /// 在控制器内获取指定视图生成后的HTML
        /// </summary>
        /// <param name="context">当前控制器的上下文</param>
        /// <param name="viewName">视图名称</param>
        /// <param name="model">视图所需要的参数</param>
        /// <returns>视图生成的HTML</returns>
        public static string GetViewHtml(this ControllerContext context, string viewName, Object param)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            context.Controller.ViewData.Model = param;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context,
                                                  viewResult.View,
                                                  context.Controller.ViewData,
                                                  context.Controller.TempData,
                                                  sw);
                try
                {
                    viewResult.View.Render(viewContext, sw);
                }
                catch (Exception ex)
                {
                    throw;
                }

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
