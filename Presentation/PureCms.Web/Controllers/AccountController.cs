using PureCms.Core.Domain.User;
using PureCms.Core.Utilities;
using PureCms.Services.Security;
using PureCms.Services.Session;
using PureCms.Services.User;
using PureCms.Web.Framework;
using PureCms.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Controllers
{
    public class AccountController : BaseWebController
    {
        private static UserService _userService = new UserService();
        private static RolePrivilegesService _rolePrivilegesService = new RolePrivilegesService();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            if (WorkContext.IsSignIn)
            {
                if (WorkContext.UrlReferrer.IsNotEmpty() && !WorkContext.UrlReferrer.IsCaseInsensitiveEqual(WorkContext.Url))
                {
                    return Redirect(WorkContext.UrlReferrer);
                }
                return Redirect("~/");
            }
            else
            {
                SignInModel model = new SignInModel();
                model.ReturnUrl = GetRouteOrQueryString("returnurl");
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInModel model)
        {
            var flag = false;
            var msg = string.Empty;
            if (ModelState.IsValid)
            {
                SessionService _session = new SessionService(this.HttpContext);
                if (!model.ValidCode.IsCaseInsensitiveEqual(_session.GetValueString("verifyCode")))
                {
                    flag = false;
                    msg = "验证码不正确";
                    ModelState.AddModelError("validcode", msg);
                }
                else {
                    UserInfo u = _userService.GetUserByLoginName(model.LoginName);
                    if (u == null)
                    {
                        flag = false;
                        msg = "帐号不存在";
                        ModelState.AddModelError("loginname", msg);
                    }
                    else
                    {
                        if (_userService.IsValidePassword(model.Password, u.Salt, u.Password))
                        {
                            //用户权限
                            u.Privileges = _rolePrivilegesService.GetAll(q => q.Where(w => w.RoleId == u.RoleId));
                            CurrentUser user = new CurrentUser();
                            user.RoleId = u.RoleId;
                            user.UserId = u.UserId;
                            user.UserName = u.Name;
                            user.Privileges = u.Privileges;
                            //加入上下文
                            WorkContext.CurrentUser = user;
                            //登录状态记录
                            _authenticationService.SignIn(u);
                            msg = "登录成功";
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                            msg = "密码不正确";
                            ModelState.AddModelError("password", msg);
                        }
                    }
                }
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(flag, msg);
            }
            else if (flag)
            {
                if (model.ReturnUrl.IsNotEmpty())
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return Redirect("~/");
                }
            }
            return View();
        }
        public ActionResult SignOut()
        {
            _authenticationService.SignOut();
            if (!WorkContext.UrlReferrer.IsEmpty() && !WorkContext.UrlReferrer.IsCaseInsensitiveEqual(WorkContext.Url))
            {
                return Redirect(WorkContext.UrlReferrer);
            }
            return Redirect("~/");
        }
    }
}
