using System;
using System.ComponentModel.DataAnnotations;

namespace PureCms.Web.Framework
{
    /// <summary>
    /// 固话号验证属性
    /// </summary>
    public class PhoneAttribute : ValidationAttribute
    {
        public PhoneAttribute()
        {
            ErrorMessage = "不是有效的固定电话号码";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            else return RegularExpressions.IsPhoneNumber.IsMatch(value.ToString());

        }
    }
}
