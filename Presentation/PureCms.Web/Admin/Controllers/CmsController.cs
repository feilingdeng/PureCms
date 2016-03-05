using PureCms.Web.Framework;
using System.Web.Mvc;
using PureCms.Core.Domain.Cms;
using PureCms.Web.Admin.Models;
using PureCms.Core.Context;
using System.EnterpriseServices;
using PureCms.Services.Cms;
using System.Linq;
using PureCms.Services.Theme;
using System;

namespace PureCms.Web.Admin.Controllers
{
    public class CmsController : BaseAdminController
    {
        private static ArticleService _articleService = new ArticleService();
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
        public ActionResult Sites(SiteModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<SiteInfo>(n => n.CreatedOn);
                model.SortDirection = (int)SortDirection.Desc;
            }

            FilterContainer<SiteInfo> container = new FilterContainer<SiteInfo>();
            if (model.IsDefault.HasValue)
            {
                container.And(n=>n.IsDefault == model.IsDefault);
            }
            if (model.IsEnabled.HasValue)
            {
                container.And(n => n.IsEnabled == model.IsEnabled);
            }
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            if (model.Theme.IsNotEmpty())
            {
                container.And(n => n.Theme == model.Theme);
            }
            if (model.Url.IsNotEmpty())
            {
                container.And(n => n.Url == model.Url);
            }
            PagedList<SiteInfo> result = _siteService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Select(c => c.SiteId, c => c.Name, c => c.CreatedOn, c => c.IsDefault, c => c.Url, c => c.IsEnabled, c => c.Theme)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            if (IsAjaxRequest)
            {
                return AjaxResult(true, model);
            }
            return View(model);
        }
        [HttpGet]
        [Description("站点编辑")]
        public ActionResult EditSite(int id = 0)
        {
            EditSiteModel model = new EditSiteModel();
            if (id > 0)
            {
                var entity = _siteService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                }
            }
            var themes = _themeService.GetAll(q => q.Sort(s => s.SortDescending(ss => ss.CreatedOn)));
            model.Themes = new SelectList(themes, "pathname", "displayname");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("站点信息保存")]
        public ActionResult EditSite(EditSiteModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new SiteInfo();
                model.CopyTo(entity);

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
                    .Where(n => n.SiteId == model.SiteId)
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
                ChannelInfo entity = _channelService.FindById(id);
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, entity);
                }
                else
                {
                    if (entity != null)
                    {
                        entity.CopyTo(model);
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("频道信息保存")]
        public ActionResult EditChannel(EditChannelModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new ChannelInfo();
                model.CopyTo(entity);

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
                var entity = _channelService.FindById(channelid);
                if (entity != null)
                {
                    model.ChannelId = entity.ChannelId;
                    model.Content = entity.Content;
                }
                else
                {
                    return PromptView(WorkContext.UrlReferrer, "请先选择频道");
                }
                var channel = _channelService.FindById(channelid);
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
                    .Where(n=>n.ChannelId == model.ChannelId)
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

        #region 文章
        [Description("文章列表")]
        public ActionResult Articles(ArticleModel model)
        {
            if (model.ChannelId <= 0)
            {
                return PromptView(WorkContext.UrlReferrer, "请先选择频道");
            }
            else
            {
                var channel = _channelService.FindById(model.ChannelId);
                model.ChannelName = channel.Name;
            }
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<ArticleInfo>(n => n.CreatedOn);
                model.SortDirection = (int)SortDirection.Desc;
            }
            FilterContainer<ArticleInfo> container = new FilterContainer<ArticleInfo>();
            if (model.Title.IsNotEmpty())
            {
                container.And(n => n.Title == model.Title);
            }
            if (model.Author.IsNotEmpty())
            {
                container.And(n => n.Author == model.Author);
            }
            if (model.IsShow.HasValue)
            {
                container.And(n => n.IsShow == model.IsShow);
            }
            if (model.ChannelId > 0)
            {
                container.And(n => n.ChannelId == model.ChannelId);
            }
            if (model.Status.HasValue)
            {
                container.And(n => n.Status == model.Status);
            }
            if (model.BeginTime.HasValue)
            {
                container.And(n => n.CreatedOn >= model.BeginTime);
            }
            if (model.EndTime.HasValue)
            {
                container.And(n => n.CreatedOn <= model.EndTime);
            }

            PagedList<ArticleInfo> result = _articleService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Select(c => c.Title, c => c.CreatedOn, c => c.Author, c => c.Url, c => c.IsShow, c => c.ChannelId)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [HttpGet]
        [Description("文章编辑")]
        public ActionResult EditArticle(int channelid, int id = 0)
        {
            EditArticleModel model = new EditArticleModel();
            if (id > 0)
            {
                var entity = _articleService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                }
                var channel = _channelService.FindById(entity.ChannelId);
                model.ChannelName = channel.Name;
                model.ChannelId = channel.ChannelId;
                channelid = channel.ChannelId;
            }
            else if (channelid <= 0)
            {
                return PromptView(WorkContext.UrlReferrer, "请先选择频道");
            }
            else
            {
                var channel = _channelService.FindById(channelid);
                model.ChannelName = channel.Name;
                model.ChannelId = channelid;
                channelid = channel.ChannelId;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("文章编辑")]
        public ActionResult EditArticle(EditArticleModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new ArticleInfo();
                model.CopyTo(entity);

                if (entity.ArticleId > 0)
                {
                    entity.UpdatedOn = DateTime.Now;
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
                foreach (var item in ModelState.Values)
                {
                    if(item.Errors.Count > 0)
                    {
                        msg += item.Errors[0].ErrorMessage + "\n";
                    }
                }
                return AjaxResult(false, "保存失败: " + msg);
            }
            return View("EditArticle", model);
        }
        [Description("删除文章")]
        [HttpPost]
        public ActionResult DeleteArticle(int[] recordid)
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
        public ActionResult SetArticleShow(int[] recordid, bool isShow)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                bool result = _articleService.Update(x => x.Set(n => n.IsShow, isShow)
                    .Where(n => n.ArticleId.In(recordid.ToList()))
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
