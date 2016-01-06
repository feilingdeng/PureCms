using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PureCms.Web.Framework
{
    public class ErrorController : BaseWebController
    {
        /// <summary>
        /// 403 页面
        /// </summary>
        public ActionResult Error403()
        {
            return View();
        }
        /// <summary>
        /// 404 页面
        /// </summary>
        public ActionResult Error404()
        {
            return View();
        }

        /// <summary>
        /// 500 页面
        /// </summary>
        public ActionResult Error500()
        {
            return View();
        }

        /// <summary>
        /// 其他错误 页面
        /// </summary>
        public ActionResult GeneralError()
        {
            return View("Error500");
        }
    }
}
