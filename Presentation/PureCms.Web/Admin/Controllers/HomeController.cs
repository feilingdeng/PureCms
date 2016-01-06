using PureCms.Core.Domain.Cms;
using PureCms.Core.Domain.Security;
using PureCms.Services.Cms;
using PureCms.Services.Security;
using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Controllers
{
    [Description("首页")]
    public class HomeController : BaseAdminController
    {
        private static ChannelService _channelService = new ChannelService();
        private static PrivilegeService _privilegeService = new PrivilegeService();

        public ActionResult Index()
        {
            return View();
        }
        [Description("无权限提示")]
        public ActionResult Unauthorized()
        {
            return View();
        }
        [Description("后台登录")]
        public ActionResult SignIn()
        {
            return View();
        }
        [Description("后台退出")]
        public ActionResult SignOut()
        {
            _authenticationService.SignOut();
            return View("SignIn");
        }
        [Description("左侧菜单")]
        [ChildActionOnly]
        public ActionResult SideBar()
        {
            var result = _channelService.GetAll(x => x
                    .Where(n => n.SiteId, 1)
                    .Where(n => n.IsEnabled, true)
                    //.Where(n => n.ContentType, ContentType.List)
                    //.Where(n => n.ContentType, ContentType.Single)
                .Sort(s => s.SortAscending(ss => ss.DisplayOrder))
                );
            ViewData["channels"] = result;
            return PartialView();
        }
    }
}
