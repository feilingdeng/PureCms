using PureCms.Services.Configuration;
using PureCms.Web.Admin.Models;
using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Controllers
{
    public class SettingsController : BaseAdminController
    {
        //private SettingService _settingService = new SettingService();
        public ActionResult Index(string name)
        {
            return View();
        }
        [HttpGet]
        [Description("编辑系统参数")]
        public ActionResult EditPlatformSetting()
        {
            PlatformSettingModel model = new PlatformSettingModel();
            PlatformSetting entity = _settingService.GetPlatformSetting();

            if (entity != null)
            {
                entity.CopyTo(model);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("编辑系统参数")]
        public ActionResult EditPlatformSetting(PlatformSettingModel model)
        {
            if (ModelState.IsValid)
            {
                PlatformSetting entity = new PlatformSetting();
                
                model.CopyTo(entity);

                _settingService.SavePlatformSetting(entity); 
                if (IsAjaxRequest)
                {
                    return AjaxResult("success", "保存成功");
                }
                return PromptView(WorkContext.UrlReferrer, "保存成功");
            }
            return View("PlatformSetting", model);
        }
    }
}
