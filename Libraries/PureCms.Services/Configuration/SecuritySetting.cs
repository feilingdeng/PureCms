using PureCms.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Services.Configuration
{
    public class SecuritySetting : ISetting
    {
        public const string CACHE_KEY = "$SecuritySetting$";
        public string AntiForgeryTokenSalt { get; set; }
    }
}
