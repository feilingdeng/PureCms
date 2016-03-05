using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PureCms.Services.Security;
using PureCms.Web.Admin.Models;
using PureCms.Core.Domain.Security;
using Newtonsoft.Json;
using PureCms.Core.Security;
using PureCms.Core.Context;
using System.EnterpriseServices;


namespace PureCms.Web.Admin.Controllers
{
    public class SecurityController : BaseAdminController
    {
        static PrivilegeService _privilegeService = new PrivilegeService();
        static RoleService _roleService = new RoleService();
        static RolePrivilegesService _rolePrivilegesService = new RolePrivilegesService();

        public ActionResult Index()
        {
            return View();
        }
        #region 权限项
        [Description("权限项")]
        public ActionResult Privileges()
        {
            if (IsAjaxRequest)
            {
                var result = _privilegeService.GetJsonData(x => x
                .Sort(s => s.SortAscending(ss => ss.DisplayOrder))
                );
                return AjaxResult(true, result);
            }
            return View();
        }

        [HttpGet]
        [Description("权限项编辑")]
        public ActionResult EditPrivilege(int id = 0)
        {
            EditPrivilegeModel model = new EditPrivilegeModel();
            if (id > 0)
            {
                PrivilegeInfo entity = _privilegeService.GetById(id);
                if (IsAjaxRequest)
                {
                    return AjaxResult(true, entity);
                }
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("权限项编辑")]
        public ActionResult EditPrivilege(EditPrivilegeModel model)
        {
            if (ModelState.IsValid)
            {
                PrivilegeInfo entity = (model.PrivilegeId.HasValue && model.PrivilegeId.Value > 0) ? _privilegeService.GetById(model.PrivilegeId.Value) : new PrivilegeInfo();
                
                model.CopyTo(entity);

                if (entity.PrivilegeId > 0)
                {
                    _privilegeService.Update(entity);
                }
                else
                {
                    entity.PrivilegeId = _privilegeService.Creat(entity);
                }
                if (WorkContext.IsHttpAjax)
                {
                    return AjaxResult(true, "保存成功");
                }
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(false, "保存失败");
            }
            return View(model);
        }
        [Description("权限项移动排序")]
        public ActionResult MovePrivilege(int moveid, int targetid, int parentid, string position)
        {
            int status = _privilegeService.Move(moveid, targetid, parentid, position);
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
            return View();
        }
        [Description("删除权限项")]
        [HttpPost]
        public ActionResult DeletePrivilege(int recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            if (IsAjaxRequest)
            {
                flag = _privilegeService.DeleteById(recordid);
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
        #endregion

        #region 角色
        [Description("角色列表")]
        public ActionResult Roles(RoleModel m)
        {
            if (m.SortBy.IsEmpty())
            {
                m.SortBy = Utilities.ExpressionHelper.GetPropertyName<RoleInfo>(n => n.Name);
                m.SortDirection = (int)SortDirection.Desc;
            }
            FilterContainer<RoleInfo> container = new FilterContainer<RoleInfo>();
            if (m.Name.IsNotEmpty())
            {
                container.And(n => n.Name == m.Name);
            }
            if (m.IsEnabled.HasValue)
            {
                container.And(n => n.IsEnabled == m.IsEnabled);
            }
            var result = _roleService.GetAll(x => x
                .Where(container)
                .Sort(n => n.OnFile(m.SortBy).ByDirection(m.SortDirection))
            );
            m.Items = result;
            if (IsAjaxRequest)
            {
                return AjaxResult(true, m);
            }
            return View(m);
        }
        [HttpGet]
        [Description("角色编辑")]
        public ActionResult EditRole(int id = 0)
        {
            EditRoleModel model = new EditRoleModel();
            if (id > 0)
            {
                var entity = _roleService.GetById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("角色编辑")]
        public ActionResult EditRole(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                RoleInfo entity = new RoleInfo();
                model.CopyTo(entity);

                if (entity.RoleId > 0)
                {
                    _roleService.Update(entity);
                }
                else
                {
                    _roleService.Creat(entity);
                }
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
            return View("editrole", model);//error
        }
        [HttpGet]
        [Description("角色权限编辑")]
        public ActionResult EditRolePrivileges(int roleId)
        {
            EditRolePrivilegesModel model = new EditRolePrivilegesModel();
            var roleInfo = _roleService.GetById(roleId);
            model.RoleName = roleInfo.Name;
            model.RoleId = roleId;

            if (IsAjaxRequest)
            {
                var result = _rolePrivilegesService.GetAll(x => x
                    .Where(n => n.RoleId == roleId)
                    .Sort(n => n.SortDescending(f=>f.PrivilegeId))
                );
                model.RolePrivileges = result;
                return AjaxResult(true, model);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("角色权限编辑")]
        public ActionResult EditRolePrivileges(EditRolePrivilegesModel m)
        {
            var roleInfo = _roleService.GetById(m.RoleId);
            m.RoleName = roleInfo.Name;
            if (ModelState.IsValid)
            {
                _rolePrivilegesService.DeleteByRoleId(m.RoleId);

                _rolePrivilegesService.CreatRolePrivileges(m.RoleId, m.PrivilegeId);
            }
            if (IsAjaxRequest)
            {
                return AjaxResult(true, "保存成功");
            }
            return View(m);
        } 
        #endregion
    }
}
