using PureCms.Core.Context;
using PureCms.Core.Domain.User;
using PureCms.Core.Utilities;
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
        public ActionResult Index(UserModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<SystemUserInfo>(n => n.CreatedOn);
                model.SortDirection = (int)SortDirection.Desc;
            }
            //model.IsActive = true; model.IsDeleted = false;
            //model.UserName = "purecms ";
            //model.MobileNumber = "13800138000";
            FilterContainer<SystemUserInfo> container = new FilterContainer<SystemUserInfo>();
            if (model.IsActive.HasValue)
            {
                container.And(f => f.IsActive == model.IsActive);
            }
            if (model.IsDeleted.HasValue)
            {
                container.And(f => f.IsDeleted == model.IsDeleted);
            }
            if (model.LoginName.IsNotEmpty())
            {
                container.And(f => f.LoginName == model.LoginName);//like
            }
            if (model.BeginTime.HasValue)
            {
                container.And(f => f.CreatedOn >= model.BeginTime.Value);
            }
            if (model.EndTime.HasValue)
            {
                container.And(f => f.CreatedOn <= model.EndTime.Value);
            }
            //container.Or(f => f.UserName == model.UserName && f.IsActive == true);
            //_userService.Query(x => x.Where(container));
            //_userService.Query(x => x.Where(filter => filter.And(f => f.IsActive == model.IsActive).And(f => f.IsDeleted == model.IsDeleted)));

            //var result = _userService.Query(x => x
            //.Page(model.Page, model.PageSize)
            //.Select(c => new { c.CreatedOn, c.LoginName, c.EmailAddress, c.IsActive, c.IsDeleted })
            //.Where(n => n.UserName == model.UserName && (n.MobileNumber.Like("138") || n.MobileNumber == model.MobileNumber)
            //&& n.UserName.In(model.UserName.TrimEnd(), "u1", "u2") && n.Gender.IsNull() && n.IsActive.IsNotNull() && n.MobileNumber.Count() >= 11)
            //.Sort(s => s.SortDescending(n => n.CreatedOn)));

            PagedList<SystemUserInfo> result = _userService.Query(x => x
                .Page(model.Page, model.PageSize)
                .Select(c => new { c.CreatedOn, c.LoginName, c.EmailAddress, c.IsActive, c.IsDeleted })
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
            );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [HttpGet]
        [Description("用户编辑")]
        public ActionResult EditUser(Guid? id)
        {
            EditUserModel model = new EditUserModel();
            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                var entity = _userService.GetById(id.Value);
                if (entity != null)
                {
                    entity.CopyTo(model);
                }
            }
            var themes = _roleService.GetAll(q => q.Sort(s => s.SortDescending(ss => ss.CreatedOn)));
            model.Roles = new SelectList(themes, "roleid", "name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("用户编辑")]
        public ActionResult EditUser(EditUserModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new SystemUserInfo();
                if (model.UserId.HasValue)
                {
                    entity = _userService.GetById(model.UserId.Value);
                    model.CopyTo(entity);
                    _userService.Update(entity);
                    msg = "保存成功";
                }
                else
                {
                    model.CopyTo(entity);
                    _userService.Create(entity);
                    msg = "新增成功";
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
        public ActionResult DeleteUser(Guid[] recordid)
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
                    .Where(n=>n.SystemUserId.In(recordid))
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


        [HttpGet]
        [Description("用户密码修改")]
        public ActionResult EditUserPassword(Guid id)
        {
            EditUserPasswordModel model = new EditUserPasswordModel();
            var entity = _userService.GetById(id);
            if (entity != null)
            {
                model.UserId = id;
                model.UserName = entity.Name;
                model.NewPassword = string.Empty;
            }
            else
            {
                return NoRecordView();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("用户密码修改")]
        public ActionResult EditUserPassword(EditUserPasswordModel model)
        {
            string msg = string.Empty;
            bool flag = false;
            if (ModelState.IsValid)
            {
                var user = _userService.GetById(model.UserId);
                string password = SecurityHelper.MD5(model.NewPassword + user.Salt);
                bool result = _userService.Update(x => x
                    .Set(n => n.Password, password)
                    .Where(n => n.SystemUserId == model.UserId)
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

            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    msg += item.Errors[0].ErrorMessage + "\n";
                }
            }
            return AjaxResult(flag, msg);
        }
    }
}
