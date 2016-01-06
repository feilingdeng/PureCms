using PureCms.Core.Configuration;

namespace PureCms.Services.Configuration
{
    public class PlatformSetting : ISetting
    {
        public const string CACHE_KEY = "$PlatformSetting$";
        public string SiteName { get; set; }
        public string Url { get; set; }

        public int Status { get; set; }

        public string ClosedReason { get; set; }
        public string ImageCDN { get; set; }
        public string CSSCDN { get; set; }
        public string ScriptCDN { get; set; }
    }
}
