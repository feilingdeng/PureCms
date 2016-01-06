using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PureCms.Web.Admin.Models
{
    //public class PrivilegeModel : BasePaged<PrivilegeInfo>
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int PrivilegeId { get; set; }
    //    public string DisplayName { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string SystemName { get; set; }
    //    public string ClassName { get; set; }
    //    public string MethodName { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int ParentPrivilegeId { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string Url { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public string OpenTarget { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public int DisplayOrder { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public bool IsEnable { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public bool IsShowAsMenu { get; set; }

    //    public string Description { get; set; }


    //    /// <summary>
    //    /// 小图标
    //    /// </summary>
    //    public string SmallIcon { get; set; }

    //    /// <summary>
    //    /// 大图标
    //    /// </summary>
    //    public string BigIcon { get; set; }

    //    public int Level { get; set; }

    //    public bool HasChild { get; set; }
    //}

    public class EditPrivilegeModel
    {

        public int? PrivilegeId { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]

        public string SystemName { get; set; }
        [Required]
        public string ClassName { get; set; }
        [Required]
        public string MethodName { get; set; }
        public int? ParentPrivilegeId { get; set; }
        [Required]
        public string Url { get; set; }
        public string OpenTarget { get; set; }
        public int? DisplayOrder { get; set; }
        [Required]
        public bool IsEnable { get; set; }
        [Required]
        public bool IsShowAsMenu { get; set; }

        public string Description { get; set; }


        /// <summary>
        /// 小图标
        /// </summary>
        public string SmallIcon { get; set; }

        /// <summary>
        /// 大图标
        /// </summary>
        public string BigIcon { get; set; }
    }
    public class RoleModel : BasePaged<RoleInfo>
    {

        public int? RoleId { get; set; }

        public string Name { get; set; }

        public bool? IsEnabled { get; set; }

        public int? ParentRoleId { get; set; }
    }

    public class EditRoleModel
    {
        [Range(0,int.MaxValue)]
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        [Range(0, int.MaxValue)]
        public int ParentRoleId { get; set; }
    }

    //public class RolePrivilegesModel : BasePaged<RolePrivilegesInfo>
    //{
    //    public int? RolePrivilegeId { get; set; }
    //    [Required]
    //    public int RoleId { get; set; }
    //    public int? PrivilegeId { get; set; }

    //    public string RoleName { get; set; }
    //    public string PrivilegeName { get; set; }
    //}

    public class EditRolePrivilegesModel
    {
        [Required]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<int> PrivilegeId { get; set; }
        public List<RolePrivilegesInfo> RolePrivileges { get; set; }
    }
}