using PureCms.Core.Plugins;
using PureCms.Utilities;
using System;
using System.Threading;
using PureCms.Utilities.Threading;
using PureCms.Services.Plugins;
using PureCms.Core;

namespace PureCms.Plugin.Pay.Alipay
{
    public class PluginCore : IPlugin
    {
        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static SettingInfo _settingInfo = null;//插件设置信息
        private static string _settingsPath = "~/plugins/PureCms.Plugin.Pay.Alipay/Settings.config";//配置文件路径

        public void Install()
        {
            throw new NotImplementedException();
        }

        public void Uninstall()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///获得插件设置
        /// </summary>
        /// <returns></returns>
        public static SettingInfo GetSettings()
        {
            if (_settingInfo == null)
            {
                using (Locker.GetWriteLock())
                {
                    if (_settingInfo == null)
                    {
                        _settingInfo = (SettingInfo)XmlHelper.DeserializeFromXML(typeof(SettingInfo),WebHelper.MapPath(_settingsPath));
                        //_settingInfo.DeserializeFromXML<SettingInfo>(out _settingInfo, WebHelper.MapPath(_settingsPath));
                    }
                }
            }
            return _settingInfo;
        }

        /// <summary>
        /// 保存插件设置
        /// </summary>
        public static void SaveSettings(SettingInfo s)
        {
            using (Locker.GetWriteLock())
            {
                s.SerializeToXml(WebHelper.MapPath(_settingsPath));
                //XmlHelper.SerializeToXml(s, WebHelper.MapPath(_settingsPath));
                _settingInfo = null;
                AlipayConfig.ReSet();
                Com.Alipay.Config.ReSet();
            }
        }
    }
}
