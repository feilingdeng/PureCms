using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "帐号不能为空")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidCode { get; set; }
        public string ReturnUrl { get; set; }
    }
}