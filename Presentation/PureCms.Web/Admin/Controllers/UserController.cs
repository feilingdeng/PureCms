using PureCms.Core.Context;
using PureCms.Core.Domain.User;
using PureCms.Services.Security;
using PureCms.Services.User;
using PureCms.Web.Admin.Models;
using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private static UserService _userService = new UserService();
        private static RoleService _roleService = new RoleService();
        

        [Description("用户列表")]
        public ActionResult Index(UserModel m)
        {
            if (m.SortBy.IsEmpty())
            {
                m.SortBy = Utilities.ExpressionHelper.GetPropertyName<UserInfo>(n => n.CreatedOn);
                m.SortDirection = (int)SortDirection.Desc;
            }

            PagedList<UserInfo> result = _userService.Query(x => x
                .Page(m.Page, m.PageSize)
                .Select(c => c.UserName, c => c.CreatedOn, c => c.LoginName, c => c.EmailAddress, c => c.IsActive, c => c.IsDeleted, c => c.MobileNumber)
                .Where(n => n.LoginName, m.LoginName).Where(n => n.IsActive, m.IsActive).Where(n => n.IsDeleted, m.IsDeleted)
                .Where(n => n.MobileNumber, m.MobileNumber)
                .Where(n => n.BeginTime, m.BeginTime).Where(n => n.EndTime, m.EndTime)
                .Sort(n => n.OnFile(m.SortBy).Sort(m.SortDirection))
                );

            m.Items = result.Items;
            m.TotalItems = result.TotalItems;
            return View(m);
        }
        [HttpGet]
        [Description("用户新增及编辑")]
        public ActionResult EditUser(int id = 0)
        {
            EditUserModel model = new EditUserModel();
            if (id > 0)
            {
                var entity = _userService.GetById(id);
                if (entity != null)
                {
                    model = typeof(UserInfo).CopyTo<EditUserModel>(entity);
                }
            }
            var themes = _roleService.GetAll(q => q.Sort(s => s.SortDescending(ss => ss.CreatedOn)));
            model.Roles = new SelectList(themes, "roleid", "name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("用户信息保存")]
        public ActionResult EditUser(EditUserModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = _userService.GetById(model.UserId);//new UserInfo();
                entity = typeof(EditUserModel).CopyTo<UserInfo>(model);

                if (entity.UserId > 0)
                {
                    _userService.Update(entity);
                    msg = "保存成功";
                }
                else
                {
                    _userService.Create(entity);
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
            return View("EditUser", model);
        }
        [Description("删除用户")]
        [HttpPost]
        public ActionResult DeleteUser(int[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                flag = _userService.DeleteById(recordid.ToList());
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
        [Description("设置用户可用状态")]
        [HttpPost]
        public ActionResult SetUserActive(int[] recordid, bool isActive)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                bool result = _userService.Update(x => x.Set(n => n.IsActive, isActive)
                    .Filter(w => w.Where(n => n.UserIdList, recordid.ToList()))
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
    }
}
