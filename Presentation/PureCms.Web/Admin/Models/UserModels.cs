using PureCms.Core.Domain.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Models
{
    public class UserModel : BasePaged<SystemUserInfo>
    {
        public int? UserId { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }

        public int? RoleId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    public class EditUserModel
    {
        public Guid? UserId { get; set; }
        public int Gender { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }
        public string Avator { get; set; }
        public string Salt { get; set; }

        public Guid RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public SelectList Roles { get; set; }
    }

    public class EditUserPasswordModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        [Required]
        [System.Web.Mvc.Compare("ConfirmPassword")]
        [DisplayName("新密码")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "名称长度为6-16")]
        public string NewPassword { get; set; }
        [Required]
        [DisplayName("确认密码")]
        public string ConfirmPassword { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    List<ValidationResult> errorList = new List<ValidationResult>();

        //    if (NewPassword != ConfirmPassword)
        //    {
        //        errorList.Add(new ValidationResult("密码不一致!", new string[] { "ConfirmPassword" }));
        //    }

        //    return errorList;
        //}
    }
}