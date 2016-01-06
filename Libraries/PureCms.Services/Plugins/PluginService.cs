using PureCms.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Services.Plugins
{
    public class PluginService
    {

        public static void Install(string systemName)
        {
            PluginManager.MarkPluginAsInstalled(systemName);
        }
        public static void UnInstall(string systemName)
        {
            PluginManager.MarkPluginAsUninstalled(systemName);
        }
        public static List<PluginDescriptor> GetInstalledPlugins()
        {
            return PluginManager.InstalledPlugins;
        }
        /// <summary>
        /// 获得插件描述对象
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        /// <returns></returns>
        public static PluginDescriptor GetPluginBySystemName(string systemName)
        {
            if (!string.IsNullOrWhiteSpace(systemName))
            {
                foreach (PluginDescriptor p in GetInstalledPlugins())
                {
                    if (p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                        return p;
                }
            }

            return null;
        }
    }
}
