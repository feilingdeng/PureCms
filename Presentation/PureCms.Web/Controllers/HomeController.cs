using PureCms.Services.Cms;
using PureCms.Web.Framework;
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
            return View();
        }

        [Description("导航菜单")]
        [ChildActionOnly]
        public ActionResult Nav()
        {
            var result = _channelService.Query(x => x
                    .Where(n => n.SiteId == 1)
                    .Where(n => n.IsEnabled == true && n.IsShow == true)
                .Sort(s => s.SortAscending(ss => ss.DisplayOrder))
                );
            ViewData["channels"] = result.Items;
            return PartialView();
        }
    }
}
