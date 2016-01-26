using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PureCms.Core.Domain.Theme;
using PureCms.Services.Theme;
using PureCms.Web.Framework;
using PureCms.Web.Models;

namespace PureCms.Web.Controllers
{
    public class ThemeController : Controller
    {
        PageService _pageService = new PageService();
        PageLayoutService _pageLayoutService = new PageLayoutService();
        PageModuleService _pageModuleService = new PageModuleService();

        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditPage(int pageId)
        {

            return View();
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPage(EditPageModel model)
        {
            if(ModelState.IsValid)
            {

            }
            return View(model);
        }
        /// <summary>
        /// 编辑布局
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditLayout(Guid? layoutId)
        {
            EditLayoutModel model = new EditLayoutModel();
            if (layoutId.HasValue)
            {
                PageLayoutInfo entity = _pageLayoutService.FindById(layoutId.Value);
                typeof(PageLayoutInfo).CopyTo<EditLayoutModel>(entity, model);
            }
            return View(model);
        }
        /// <summary>
        /// 编辑布局
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLayout(EditLayoutModel model)
        {
            if (ModelState.IsValid)
            {
                bool isUpdate = model.LayoutId.HasValue;
                PageLayoutInfo entity;
                if (isUpdate)
                {
                    entity = _pageLayoutService.FindById(model.LayoutId.Value);
                    typeof(EditLayoutModel).CopyTo<PageLayoutInfo>(model, entity);
                    _pageLayoutService.Update(entity);
                }
                else {
                    entity = new PageLayoutInfo();
                    entity.LayoutId = Guid.NewGuid();
                    typeof(EditLayoutModel).CopyTo<PageLayoutInfo>(model, entity);
                    _pageLayoutService.Create(entity);
                }
            }
            return View(model);
        }
        /// <summary>
        /// 编辑布局
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditModule(Guid? moduleId)
        {

            return View();
        }
        /// <summary>
        /// 编辑布局
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModule(EditModuleModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}
