using System.Web.Mvc;

namespace PureCms.Web.Admin
{
    public class AdminArea : System.Web.Mvc.AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}",
                              new { controller = "home", action = "index", area = "admin" },
                              new[] { "PureCms.Web.Admin.Controllers" }
            );
        }
    }
}
