using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Framework
{
    public class UserControlController : Controller
    {

        public ActionResult GridView(GridView grid)
        {
            return View("gridview", grid);
        }

    }
}
