using System.Web.Mvc;

namespace PureCms.Web.Framework
{
    public static class UserControlExtensions
    {

        public static ActionResult GridView(this HtmlHelper helper, GridView grid)
        {
            return new UserControlController().GridView(grid);
        }
    }
}
