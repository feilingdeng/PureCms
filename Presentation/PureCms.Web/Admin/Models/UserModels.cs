using PureCms.Core.Domain.User;
using System;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Models
{
    public class UserModel : BasePaged<UserInfo>
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
        public int UserId { get; set; }
        public int Gender { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }
        public string Avator { get; set; }
        public string Salt { get; set; }

        public int RoleId { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public SelectList Roles { get; set; }
    }

    public class EditUserPasswordModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}