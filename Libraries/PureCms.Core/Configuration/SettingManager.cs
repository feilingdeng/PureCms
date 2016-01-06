using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using PureCms.Utilities;

namespace PureCms.Core.Configuration
{
    public class SettingManager
    {
        private static string _settingsPath = CommonHelper.GetAppSetting("PureCms.SettingsPath");

        public bool Save<T>(T t) where T : ISetting
        {
            t.SerializeToXml(GetSavePath<T>());
            return true;
        }

        public T Get<T>() where T : ISetting
        {
            Type tt = typeof(T);
            return (T)XmlHelper.DeserializeFromXML(tt, GetSavePath<T>());
        }

        private string GetSavePath<T>()
        {
            Type tt = typeof(T);
            return WebHelper.MapPath(_settingsPath + tt.Name + ".config");
        }
    }
}
