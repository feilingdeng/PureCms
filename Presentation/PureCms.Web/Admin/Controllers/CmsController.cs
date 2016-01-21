using PureCms.Web.Framework;
using System.Web.Mvc;
using PureCms.Core.Domain.Cms;
using PureCms.Web.Admin.Models;
using PureCms.Core.Context;
using System.EnterpriseServices;
using PureCms.Services.Cms;
using System.Linq;

namespace PureCms.Web.Admin.Controllers
{
    public class CmsController : BaseAdminController
    {
        private static ArticleService _articleService = new ArticleService();
        private static ArticleCategoryService _articleCategoryService = new ArticleCategoryService();
        private static SiteService _siteService = new SiteService();
        private static ThemeService _themeService = new ThemeService();
        private static ChannelService _channelService = new ChannelService();

        //
        // GET: /Cms/

        public ActionResult Index()
        {
            return RedirectToAction("Sites");
        }

        #region 站点
        [Description("站点列表")]
        public ActionResult Sites(SiteModel m)
        {
            if (m.SortBy.IsEmpty())
            {
                m.SortBy = Utilities.ExpressionHelper.GetPropertyName<SiteInfo>(n => n.CreatedOn);
                m.SortDirection = (int)SortDirection.Desc;
            }

            PagedList<SiteInfo> result = _siteService.Query(x => x
                .Page(m.Page, m.PageSize)
                .Select(c => c.SiteId, c => c.Name, c => c.CreatedOn, c => c.IsDefault, c => c.Url, c => c.IsEnabled, c => c.Theme)
                .Where(n => n.IsDefault, m.IsDefault).Where(n => n.IsEnabled, m.IsEnabled)
                .Where(n => n.Name, m.Name).Where(n => n.Theme, m.Theme)
                .Where(n => n.Url, m.Url)
                .Sort(n => n.OnFile(m.SortBy).Sort(m.SortDirection))
                );

            m.Items = result.Items;
            m.TotalItems = result.TotalItems;
            if (IsAjaxRequest)
            {
                return AjaxResult(true, m);
            }
            return View(m);
        }
        [HttpGet]
        [Description("站点编辑")]
        public ActionResult EditSite(int id = 0)
        {
            EditSiteModel model = new EditSiteModel();
            if (id > 0)
            {
                var entity = _siteService.GetById(id);
                if (entity != null)
                {
                    model = typeof(SiteInfo).CopyTo<EditSiteModel>(entity);
                }
            }
            var themes = _themeService.GetAll(q => q.Sort(s => s.SortDescending(ss => ss.CreatedOn)));
            model.Themes = new SelectList(themes, "pathname", "displayname");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("站点信息保存")]
        public ActionResult SaveSite(EditSiteModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new SiteInfo();
                entity = typeof(EditSiteModel).CopyTo<SiteInfo>(model);

                if (entity.SiteId > 0)
                {
                    _siteService.Update(entity);
                    msg = "保存成功";
                }
                else
                {
                    _siteService.Create(entity);
                    msg = "创建成功";
                }
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, msg);
                }
                return PromptView(WorkContext.UrlReferrer, msg);
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(false, "保存失败");
            }
            return View("EditSite", model);
        }
        #endregion

        #region 频道
        [Description("频道列表")]
        public ActionResult Channels(ChannelModel model, bool wrapRoot = true)
        {
            if (IsAjaxRequest)
            {
                var result = _channelService.GetJsonData(x => x
                    .Where(n => n.SiteId, model.SiteId)
                .Sort(s => s.SortAscending(ss => ss.DisplayOrder))
                , wrapRoot: wrapRoot
                );
                return AjaxResult(true, result);
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditChannel(int id = 0)
        {
            EditChannelModel model = new EditChannelModel();
            if (id > 0)
            {
                ChannelInfo entity = _channelService.GetById(id);
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, entity);
                }
                else
                {
                    if (entity != null)
                    {
                        model = typeof(ChannelInfo).CopyTo<EditChannelModel>(entity);
                    }
                }
            }

            return View(model);
        }
        //[HttpGet]
        //[Description("频道编辑")]
        //public ActionResult EditChannel(int id = 0)
        //{
        //    EditChannelModel model = new EditChannelModel();
        //    if (id > 0)
        //    {
        //        var entity = _siteService.GetById(id);
        //        if (entity != null)
        //        {
        //            model = typeof(ChannelInfo).CopyTo<EditChannelModel>(entity);
        //        }
        //    }
        //    return View(model);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("频道信息保存")]
        public ActionResult EditChannel(EditChannelModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new ChannelInfo();
                entity = typeof(EditChannelModel).CopyTo<ChannelInfo>(model);

                if (entity.ChannelId > 0)
                {
                    _channelService.Update(entity);
                    msg = "保存成功";
                }
                else
                {
                    _channelService.Create(entity);
                    msg = "创建成功";
                }
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, msg);
                }
                return PromptView(WorkContext.UrlReferrer, msg);
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(false, "保存失败");
            }
            return View("EditChannel", model);
        }
        public ActionResult MoveChannel(int moveid, int targetid, int parentid, string position)
        {
            int status = _channelService.Move(moveid, targetid, parentid, position);
            if (IsAjaxRequest)
            {
                if (status == 1)
                {
                    return AjaxResult(true, "保存成功");
                }
                else
                {
                    return AjaxResult(true, "保存失败");
                }
            }
            return RedirectToAction("Channels");
        }
        [Description("删除频道")]
        [HttpPost]
        public ActionResult DeleteChannel(int recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                flag = _channelService.DeleteById(recordid);
                if (flag)
                {
                    msg = "删除成功";
                }
                else
                {
                    msg = "删除失败";
                }
                return AjaxResult(flag, msg);
            }
            return PromptView(WorkContext.UrlReferrer, msg);
        }
        [Description("单页内容发布")]
        public ActionResult EditPageContent(int channelid)
        {
            EditContentModel model = new EditContentModel();
            if (channelid <= 0)
            {
                return PromptView(WorkContext.UrlReferrer, "请先选择频道");
            }
            else
            {
                var entity = _channelService.GetById(channelid);
                if (entity != null)
                {
                    model.ChannelId = entity.ChannelId;
                    model.Content = entity.Content;
                }
                else
                {
                    return PromptView(WorkContext.UrlReferrer, "请先选择频道");
                }
                var channel = _channelService.GetById(channelid);
                model.ChannelName = channel.Name;
                model.ChannelId = channelid;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("单页内容发布")]
        public ActionResult EditPageContent(EditContentModel model)
        {
            if (ModelState.IsValid)
            {
                _channelService.Update(x => x.Set(n => n.Content, model.Content)
                    .Filter(w => w.Where(n => n.ChannelId, model.ChannelId))
                    );
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, "保存成功");
                }
                return PromptView(WorkContext.UrlReferrer, "保存成功");
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(false, "保存失败");
            }
            return View(model);
        }
        #endregion


        #region 分类
        //[Description("分类列表")]
        //public ActionResult Categories(ArticleCategoryModel m)
        //{
        //    if (m.SortBy.IsEmpty())
        //    {
        //        m.SortBy = Utilities.ExpressionHelper.GetPropertyName<ArticleCategoryInfo>(n => n.CreatedOn);
        //        m.SortDirection = (int)SortDirection.Desc;
        //    }

        //    //PagedList<ArticleCategoryInfo> result = _articleCategoryService.Query(x => x
        //    //    .Page(m.Page, m.PageSize)
        //    //    //.Select(c => c.Name, c => c.CreatedOn, c => c.IsDefault, c => c.Url, c => c.IsEnabled, c => c.Theme)
        //    //    //.Where(n => n.ChannelId, m.c).Where(n => n.IsEnabled, m.IsEnabled)
        //    //    .Where(n => n.Name, m.Name)
        //    //    .Where(n => n.Url, m.Url)
        //    //    .Sort(n => n.OnFile(m.SortBy).Sort(m.SortDirection))
        //    //    );

        //    //m.Items = result.Items;
        //    //m.TotalItems = result.TotalItems;
        //    return View(m);
        //}
        //[HttpGet]
        //[Description("分类编辑")]
        //public ActionResult EditCategory(int id = 0)
        //{
        //    EditSiteModel model = new EditSiteModel();
        //    if (id > 0)
        //    {
        //        var entity = _siteService.GetById(id);
        //        if (entity != null)
        //        {
        //            model = typeof(SiteInfo).CopyTo<EditSiteModel>(entity);
        //        }
        //    }
        //    var themes = _themeService.GetAll(q => q.Sort(s => s.SortDescending(ss => ss.CreatedOn)));
        //    model.Themes = new SelectList(themes, "pathname", "displayname");
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //[Description("分类信息保存")]
        //public ActionResult SaveCategory(EditSiteModel model)
        //{
        //    string msg = string.Empty;
        //    if (ModelState.IsValid)
        //    {
        //        var entity = new SiteInfo();
        //        entity = typeof(EditSiteModel).CopyTo<SiteInfo>(model);

        //        if (entity.SiteId > 0)
        //        {
        //            _siteService.Update(entity);
        //            msg = "保存成功";
        //        }
        //        else
        //        {
        //            _siteService.Creat(entity);
        //            msg = "创建成功";
        //        }
        //        if (IsAjaxRequest)
        //        {
        //            return AjaxResult("success", msg);
        //        }
        //        return PromptView(WorkContext.UrlReferrer, msg);
        //    }
        //    if (IsAjaxRequest)
        //    {
        //        return AjaxResult("error", "保存失败");
        //    }
        //    return View("EditSite", model);
        //}
        #endregion

        #region 文章
        [Description("文章列表")]
        public ActionResult Articles(ArticleModel m)
        {
            if (m.ChannelId <= 0)
            {
                return PromptView(WorkContext.UrlReferrer, "请先选择频道");
            }
            else
            {
                var channel = _channelService.GetById(m.ChannelId);
                m.ChannelName = channel.Name;
            }
            if (m.SortBy.IsEmpty())
            {
                m.SortBy = Utilities.ExpressionHelper.GetPropertyName<ArticleInfo>(n => n.CreatedOn);
                m.SortDirection = (int)SortDirection.Desc;
            }

            PagedList<ArticleInfo> result = _articleService.Query(x => x
                .Page(m.Page, m.PageSize)
                .Select(c => c.Title, c => c.CreatedOn, c => c.Author, c => c.Url, c => c.IsShow)
                .Where(n => n.Title, m.Title).Where(n => n.Author, m.Author).Where(n => n.IsShow, m.IsShow)//.Where(n => n.CategoryId, m.CategoryId)
                .Where(n => n.ChannelId, m.ChannelId).Where(n => n.Status, m.Status)
                .Where(n => n.BeginTime, m.BeginTime).Where(n => n.EndTime, m.EndTime)
                .Sort(n => n.OnFile(m.SortBy).Sort(m.SortDirection))
                );

            m.Items = result.Items;
            m.TotalItems = result.TotalItems;
            return View(m);
        }
        [HttpGet]
        [Description("文章发布")]
        public ActionResult EditArticle(int channelid, long id = 0)
        {
            EditArticleModel model = new EditArticleModel();
            if (id > 0)
            {
                var entity = _articleService.GetById(id);
                if (entity != null)
                {
                    model = typeof(ArticleInfo).CopyTo<EditArticleModel>(entity);
                }
                var channel = _channelService.GetById(entity.ChannelId);
                model.ChannelName = channel.Name;
            }
            else if (channelid <= 0)
            {
                return PromptView(WorkContext.UrlReferrer, "请先选择频道");
            }
            else
            {
                var channel = _channelService.GetById(channelid);
                model.ChannelName = channel.Name;
                model.ChannelId = channelid;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("文章信息保存")]
        public ActionResult SaveArticle(EditArticleModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new ArticleInfo();
                entity = typeof(EditArticleModel).CopyTo<ArticleInfo>(model);

                if (entity.ArticleId > 0)
                {
                    _articleService.Update(entity);
                    msg = "保存成功";
                }
                else
                {
                    _articleService.Create(entity);
                    msg = "发布成功";
                }
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, msg);
                }
                return PromptView(WorkContext.UrlReferrer, msg);
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(false, "保存失败");
            }
            return View("EditArticle", model);
        }
        [Description("删除文章")]
        [HttpPost]
        public ActionResult DeleteArticle(long[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                flag = _articleService.DeleteById(recordid.ToList());
                if (flag)
                {
                    msg = "删除成功";
                }
                else
                {
                    msg = "删除失败";
                }
                return AjaxResult(flag, msg);
            }
            return PromptView(WorkContext.UrlReferrer, msg);
        }
        [Description("设置文章显示状态")]
        [HttpPost]
        public ActionResult SetArticleShow(long[] recordid, bool isShow)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                bool result = _articleService.Update(x => x.Set(n => n.IsShow, isShow)
                    .Filter(w => w.Where(n => n.ArticleIdList, recordid.ToList()))
                    );
                flag = result;
                if (flag)
                {
                    msg = "更新成功";
                }
                else
                {
                    msg = "更新失败";
                }
                return AjaxResult(flag, msg);
            }
            return PromptView(WorkContext.UrlReferrer, msg);
        }
        #endregion
    }
}
