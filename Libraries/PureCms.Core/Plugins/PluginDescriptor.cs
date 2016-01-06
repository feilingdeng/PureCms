using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PureCms.Core.Plugins
{
    public class PluginDescriptor : IComparable<PluginDescriptor>
    {
        public PluginDescriptor()
        {
            this.Version = new Version("1.0");
            this.MinVersion = new Version(PureCmsVersion.VERSION);
        }

        /// <summary>
        /// Gets or sets the ClassFullName
        /// </summary>
        public string ClassFullName { get; set; }

        /// <summary>
        /// Gets or sets the Namespace
        /// </summary>
        public string Namespace { get; set; }


        /// <summary>
        /// Gets or sets the plugin group
        /// </summary>
        //[DataMember]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        //[DataMember]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets the folder name
        /// </summary>
        //[DataMember]
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the system name
        /// </summary>
        //[DataMember]
        public string SystemName { get; set; }

        /// <summary>
        /// Gets the plugin description
        /// </summary>
        //[DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        //[DataMember]
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the minimum supported app version
        /// </summary>
        //[DataMember]
        public Version MinVersion { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        //[DataMember]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the project/marketplace url
        /// </summary>
        //[DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        //[DataMember]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether plugin is installed
        /// </summary>
        //[DataMember]
        public bool Installed { get; set; }
        /// <summary>
        /// 配置入口
        /// </summary>
        public string ConfigureControllerName { get; set; }

        /// <summary>
        /// 配置方法
        /// </summary>
        public string ConfigureActionName { get; set; }

        /// <summary>
        /// 调用入口
        /// </summary>
        public string EntryControllerName { get; set; }

        /// <summary>
        /// 调用方法
        /// </summary>
        public string EntryActionName { get; set; }

        /// <summary>
        /// 插件实例
        /// </summary>
        private IPlugin _instance = null;

        /// <summary>
        /// 插件实例
        /// </summary>
        [XmlIgnoreAttribute]
        public IPlugin Instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        _instance = (IPlugin)Activator.CreateInstance(System.Type.GetType(ClassFullName, false, true));
                    }
                    catch (Exception ex)
                    {
                        Error.Application("创建插件\"" + ClassFullName + "\"实例失败，详细信息：{0}", ex.Message);
                    }
                }
                return _instance;
            }
        }

        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);
            else if (FriendlyName != null)
                return FriendlyName.CompareTo(other.FriendlyName);
            return 0;
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }
    }
}
