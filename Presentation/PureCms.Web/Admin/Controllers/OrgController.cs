using PureCms.Web.Framework;
using System.Web.Mvc;
using PureCms.Core.Domain.Logging;
using PureCms.Web.Admin.Models;
using PureCms.Core.Context;
using System.Collections.Generic;

namespace PureCms.Web.Admin.Controllers
{
    public class OrgController : BaseAdminController
    {
        //
        // GET: /Org/

        public ActionResult Index()
        {
            return View();
        }

    }
}
