using PureCms.Services.Common;
using PureCms.Utilities;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using PureCms.Core;
using PureCms.Web.Framework.Mvc;

namespace PureCms.Web.Admin.Controllers
{
    [AuthorizeFilter]
    public class ToolController : Controller
    {
        //protected override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //}
        /// <summary>
        /// 上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string op)
        {
            UploadResult result = new UploadResult();
            if (op == "product")//上传商品图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string fileName = new Uploader().SaveUplaodProductImage(file);
                result.SetMessage(fileName, "/upload/product/thumb800_800/" + fileName);
            }
            else if (op == "article")//上传文章图片
            {
                HttpPostedFileBase file = Request.Files[0];
                string fileName = new Uploader().SaveUplaodArticleImage(file);
                result.SetMessage(fileName, "/upload/cms/thumb800_800/" + fileName);
            }
            else {
                result.error = 1;
                result.message = "上传失败";
            }
            return new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

    public class UploadResult
    {
        public int error { get; set; }

        public string url { get; set; }

        public string message { get; set; }

        public void SetMessage(string _status, string _url)
        {
            if (_status == "-1")
            {
                this.error = 1;
                this.message = "上传失败：请选择文件";
            }
            else if (_status == "-2")
            {
                this.error = 1;
                this.message = "上传失败：图片格式错误";
            }
            else if (_status == "-3")
            {
                this.error = 1;
                this.message = "上传失败：文件太大";
            }
            else {
                this.error = 0;
                this.url = _url.Replace("\\", "/");
            }
        }
    }
}
