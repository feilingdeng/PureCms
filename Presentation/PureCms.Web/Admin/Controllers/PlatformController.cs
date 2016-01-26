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
            FilterContainer<LogInfo> container = new FilterContainer<LogInfo>();
            if (m.ClientIp.IsNotEmpty())
            {
                container.And(n => n.ClientIP == m.ClientIp);
            }
            if (m.Url.IsNotEmpty())
            {
                container.And(n => n.Url == m.Url);
            }
            if (m.BeginTime.HasValue)
            {
                container.And(n => n.CreatedOn >= m.BeginTime);
            }
            if (m.EndTime.HasValue)
            {
                container.And(n => n.CreatedOn <= m.EndTime);
            }

            PagedList<LogInfo> result = _logService.Query(x => x
                .Page(page, pageSize)
                .Select(c => c.Title, c => c.CreatedOn, c => c.ClientIP, c => c.UserName, c => c.Url)
                .Where(container)
                .Sort(n => n.OnFile(m.SortBy).ByDirection(m.SortDirection))
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
