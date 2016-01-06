using PureCms.Core.Configuration;

namespace PureCms.Services.Configuration
{
    public class SecuritySetting : ISetting
    {
        public const string CACHE_KEY = "$SecuritySetting$";
        public string AntiForgeryTokenSalt { get; set; }
    }
}
