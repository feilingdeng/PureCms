using PureCms.Core.Caching;
using PureCms.Core.Configuration;

namespace PureCms.Services.Configuration
{
    public class SettingService
    {
        private static ICache _cache = new AspNetCache();
        private static SettingManager _settingManager = new SettingManager();
        public PlatformSetting GetPlatformSetting()
        {
            PlatformSetting s = new PlatformSetting();
            if (_cache.Contains(PlatformSetting.CACHE_KEY))
            {
                s = (PlatformSetting)_cache.Get(PlatformSetting.CACHE_KEY);
            }
            else
            {
                s = _settingManager.Get<PlatformSetting>();
                _cache.Set(PlatformSetting.CACHE_KEY, s, null);
            }
            return s;
        }

        public void SavePlatformSetting(PlatformSetting s)
        {
            _settingManager.Save<PlatformSetting>(s);
            if (_cache.Contains(PlatformSetting.CACHE_KEY))
            {
                _cache.Set(PlatformSetting.CACHE_KEY, s, null);
            }
        }
        public SecuritySetting GetSecuritySetting()
        {
            SecuritySetting s = new SecuritySetting();
            if (_cache.Contains(SecuritySetting.CACHE_KEY))
            {
                s = (SecuritySetting)_cache.Get(SecuritySetting.CACHE_KEY);
            }
            else
            {
                s = _settingManager.Get<SecuritySetting>();
                _cache.Set(SecuritySetting.CACHE_KEY, s, null);
            }
            return s;
        }

        public void SaveSecuritySetting(SecuritySetting s)
        {
            _settingManager.Save<SecuritySetting>(s);
            if (_cache.Contains(SecuritySetting.CACHE_KEY))
            {
                _cache.Set(SecuritySetting.CACHE_KEY, s, null);
            }
        }
    }
}
