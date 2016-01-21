using PureCms.Web.Framework;
using System.Web.Mvc;
using PureCms.Core.Domain.Logging;
using PureCms.Web.Admin.Models;
using PureCms.Core.Context;
using System.Collections.Generic;

namespace PureCms.Web.Admin.Controllers
{
    public class PlatformController : BaseAdminController
    {
        //
        // GET: /Platform/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Log(LogModel m)
        {
            int page = m.Page, pageSize = m.PageSize;
            if (m.SortBy.IsEmpty())
            {
                m.SortBy = Utilities.ExpressionHelper.GetPropertyName<LogInfo>(n => n.CreatedOn);
                m.SortDirection = (int)SortDirection.Desc;
            }

            PagedList<LogInfo> result = _logService.Query(x => x
                .Page(page, pageSize)
                .Select(c => c.Title, c => c.CreatedOn, c => c.ClientIP, c => c.UserName, c => c.Url)
                .Where(n => n.ClientIP, m.ClientIp).Where(n => n.Url, m.Url).Where(n => n.BeginTime, m.BeginTime).Where(n => n.EndTime, m.EndTime)
                .Sort(n => n.OnFile(m.SortBy).Sort(m.SortDirection))
                );

            m.Items = result.Items;
            m.TotalItems = result.TotalItems;

            return View(m);
        }
        public ActionResult DeleteLog(List<int> recordid)
        {

            if (IsAjaxRequest)
            {
                return AjaxResult("success", "删除成功");
            }
            return PromptView(WorkContext.UrlReferrer, "删除成功");
        }
    }
}
