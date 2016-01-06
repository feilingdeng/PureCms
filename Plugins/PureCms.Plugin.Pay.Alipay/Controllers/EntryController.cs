using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PureCms.Plugin.Pay.Alipay.Controllers
{
    public class EntryController : Controller
    {
        public ActionResult Entry(string orders)
        {
            ViewData["orders"] = orders;
            return View();
        }
    }
}
