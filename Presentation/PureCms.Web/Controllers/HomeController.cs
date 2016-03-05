using PureCms.Services.Cms;
using PureCms.Services.Query;
using PureCms.Web.Framework;
using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace PureCms.Web.Controllers
{
    public class HomeController : BaseWebController
    {
        private static ChannelService _channelService = new ChannelService();
        //
        // GET: /Home/
        [Description("首页")]
        public ActionResult Index()
        {
            //throw new Exception("error");
            //List<string> result = AuthenticationService.GetAllActionByAssembly();
            //UserInfo u = _authenticationService.GetAuthenticatedUser();

            var _fetchService = new FetchDataService();
            //var s = _fetchService.ToJsonString();
            var o = new QueryViewService().FindById(Guid.Parse("00000000-0000-0000-00aa-000010001002"));
            var sql = _fetchService.ToSqlString(_fetchService.ToQueryExpression(o.FetchConfig));
            //new QueryViewService().Update(x=>x.Set(f=>f.FetchConfig, s).Where(w=>w.QueryViewId == Guid.Parse("00000000-0000-0000-00aa-000010001002")));
            return View();
        }

        [Description("导航菜单")]
        [ChildActionOnly]
        public ActionResult Nav()
        {
            var result = _channelService.QueryPaged(x => x
                    .Where(n => n.SiteId == 1)
                    .Where(n => n.IsEnabled == true && n.IsShow == true)
                .Sort(s => s.SortAscending(ss => ss.DisplayOrder))
                );
            ViewData["channels"] = result.Items;
            return PartialView();
        }
    }
}
