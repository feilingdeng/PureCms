using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PureCms.Plugin.Pay.Alipay.Controllers
{
    public class ConfigureController : Controller
    {
        /// <summary>
        /// 配置
        /// </summary>
        [HttpGet]
        //[ChildActionOnly]
        public ActionResult Configure()
        {
            SettingModel model = new SettingModel();

            SettingInfo stInfo = PluginCore.GetSettings();
            model.Partner = stInfo.Partner;
            model.Key = stInfo.Key;
            model.PrivateKey = stInfo.PrivateKey;
            model.Seller = stInfo.Seller;
            model.PayFee = stInfo.PayFee;
            model.FreeMoney = stInfo.FreeMoney;

            return View(model);
        }
        /// <summary>
        /// 配置
        /// </summary>
        [HttpPost]
        public ActionResult Configure(SettingModel model)
        {
            if (ModelState.IsValid)
            {
                SettingInfo stInfo = new SettingInfo();
                stInfo.Partner = model.Partner.Trim();
                stInfo.Key = model.Key.Trim();
                stInfo.PrivateKey = model.PrivateKey.Trim();
                stInfo.Seller = model.Seller.Trim();
                stInfo.PayFee = model.PayFee;
                stInfo.FreeMoney = model.FreeMoney;
                PluginCore.SaveSettings(stInfo);

                //AddMallAdminLog("修改支付宝插件配置信息");
                //return PromptView(Url.Action("config", "plugin", new { configController = "AdminAlipay", configAction = "Config" }), "插件配置修改成功");
                ViewData["msg"] = "ok";
                return View();
            }
            ViewData["msg"] = "not ok";
            return View();
            //return PromptView(Url.Action("config", "plugin", new { configController = "AdminAlipay", configAction = "Config" }), "信息有误，请重新填写");
        }
    }
}
