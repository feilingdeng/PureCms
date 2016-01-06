using PureCms.Core.Plugins;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PureCms.Utilities.Threading;
using PureCms.Utilities;
using System.Web.Compilation;
using System.Diagnostics;

[assembly: PreApplicationStartMethod(typeof(PluginManager), "Initialize")]
namespace PureCms.Core.Plugins
{
    /// <summary>
    /// Sets the application up for the plugin referencing
    /// </summary>
    public class PluginManager
    {
        #region Fields

        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static readonly string _pluginsPath = "~/Plugins";
        private static readonly string _shadowCopyPath = "~/Plugins/bin";
        private static readonly string _installedPluginsPath = "~/App_Data/InstalledPlugins.xml";
        private static readonly bool _clearShadowDirectoryOnStartup = true;
        private static List<string> _installedPluginSystemNames = new List<string>();
        private static List<PluginDescriptor> _installedPlugins = new List<PluginDescriptor>();//已安装插件列表
        private static List<PluginDescriptor> _unInstalledPlugins = new List<PluginDescriptor>();//未安装插件列表


        /// <summary>
        /// 插件列表
        /// </summary>
        public static List<PluginDescriptor> InstalledPlugins
        {
            get { return _installedPlugins; }
        }
        /// <summary>
        /// 未安装插件列表
        /// </summary>
        public static List<PluginDescriptor> UnInstalledPlugins
        {
            get { return _unInstalledPlugins; }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initialize
        /// </summary>
        public static void Initialize()
        {
            // adding a process-specific environment path (either bin/x86 or bin/amd64)
            // ensures that unmanaged native dependencies can be resolved successfully.
            SetPrivateEnvPath();


            using (Locker.GetWriteLock())
            {
                // TODO: Add verbose exception handling / raising here since this is happening on app startup and could
                // prevent app from starting altogether
                DirectoryInfo _pluginFolder = new DirectoryInfo(WebHelper.MapPath(_pluginsPath));
                DirectoryInfo _shadowCopyFolder = new DirectoryInfo(WebHelper.MapPath(_shadowCopyPath));
                try
                {
                    //ensure folders are created
                    Directory.CreateDirectory(_pluginFolder.FullName);
                    Directory.CreateDirectory(_shadowCopyFolder.FullName);

                    // get list of all files in bin
                    var binFiles = _shadowCopyFolder.GetFiles("*", SearchOption.AllDirectories);
                    if (_clearShadowDirectoryOnStartup)
                    {
                        // clear out shadow copied plugins
                        foreach (var f in binFiles)
                        {
                            try
                            {
                                File.Delete(f.FullName);
                            }
                            catch (Exception exc)
                            {
                            }
                        }
                    }

                    // determine all plugin folders
                    var pluginPaths = from x in Directory.EnumerateDirectories(_pluginFolder.FullName)
                                      where !x.IsMatch("bin") && !x.IsMatch("_Backup")
                                      select Path.Combine(_pluginFolder.FullName, x);

                    _installedPluginSystemNames = GetInstalledPluginSystemNameList();

                    // now activate all plugins
                    foreach (var pluginPath in pluginPaths)
                    {
                        var result = LoadPlugin(pluginPath, _installedPluginSystemNames);
                        if (IsInstalledlPlugin(result.SystemName, _installedPluginSystemNames))//安装的插件
                        {
                            _installedPlugins.Add(result);
                        }
                        else
                        {
                            _unInstalledPlugins.Add(result);
                        }
                    }

                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                    {
                        msg += e.Message + Environment.NewLine;
                    }

                    var fail = new Exception(msg, ex);
                    Debug.WriteLine(fail.Message, fail);

                    throw fail;
                }

            }
        }

        private static PluginDescriptor LoadPlugin(string pluginFolderPath, ICollection<string> installedPlugins)
        {
            Guard.ArgumentNotEmpty(() => pluginFolderPath);

            var folder = new DirectoryInfo(pluginFolderPath);
            if (!folder.Exists)
            {
                return null;
            }

            var descriptionFile = new FileInfo(Path.Combine(pluginFolderPath, "plugin.config"));
            if (!descriptionFile.Exists)
            {
                return null;
            }

            // load descriptor file (Description.txt)
            var descriptor = GetPluginDescriptor(descriptionFile.FullName);

            // some validation
            if (descriptor.SystemName.IsEmpty())
            {
                throw new Exception("The plugin descriptor '{0}' does not define a plugin system name. Try assigning the plugin a unique name and recompile.".FormatInvariant(descriptionFile.FullName));
            }
            if (descriptor.ClassFullName.IsEmpty())
            {
                throw new Exception("The plugin descriptor '{0}' does not define a plugin assembly file name. Try assigning the plugin a file name and recompile.".FormatInvariant(descriptionFile.FullName));
            }


            try
            {
                // get list of all DLLs in plugin folders (not in 'bin' or '_Backup'!)
                var pluginBinaries = descriptionFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                    // just make sure we're not registering shadow copied plugins
                    .Where(x => IsPackagePluginFolder(x.Directory))
                    .ToList();

                // other plugin description info
                var mainPluginFile = pluginBinaries.Where(x => x.Name.IsCaseInsensitiveEqual(descriptor.Namespace + ".dll")).FirstOrDefault();
                Probe(mainPluginFile);

                // load all other referenced assemblies now
                var otherAssemblies = from x in pluginBinaries
                                      where !x.Name.IsCaseInsensitiveEqual(mainPluginFile.Name)
                                      select x;

                foreach (var assemblyFile in otherAssemblies)
                {
                    if (!IsAlreadyLoaded(assemblyFile))
                    {
                        Probe(assemblyFile);
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                var msg = string.Empty;
                foreach (var e in ex.LoaderExceptions)
                {
                    msg += e.Message + Environment.NewLine;
                }

                var fail = new Exception(msg, ex);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }

            return descriptor;
        }
        public static PluginDescriptor GetPluginDescriptor(string filePath)
        {
            return (PluginDescriptor)XmlHelper.DeserializeFromXML(typeof(PluginDescriptor), filePath);
        }
        /// <summary>
        /// Mark plugin as installed
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            Guard.ArgumentNotEmpty(() => systemName);

            using (Locker.GetWriteLock())
            {
                if (string.IsNullOrWhiteSpace(systemName))
                    return;

                //在未安装的插件列表中获得对应插件
                PluginDescriptor descriptor = _unInstalledPlugins.Find(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));

                //当插件为空时直接返回
                if (descriptor == null)
                    return;

                //当插件不为空时将插件添加到相应列表
                _installedPlugins.Add(descriptor);
                _installedPlugins.Sort((first, next) => first.DisplayOrder.CompareTo(next.DisplayOrder));

                //在未安装的插件列表中移除对应插件
                _unInstalledPlugins.Remove(descriptor);

                //将新安装的插件保存到安装的插件列表中
                _installedPluginSystemNames.Add(descriptor.SystemName);
                SaveInstalledPluginSystemNameList(_installedPluginSystemNames);
            }
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            Guard.ArgumentNotEmpty(() => systemName);

            using (Locker.GetWriteLock())
            {
                if (string.IsNullOrEmpty(systemName))
                    return;

                PluginDescriptor descriptor = null;
                Predicate<PluginDescriptor> condition = x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase);
                descriptor = _installedPlugins.Find(condition);

                //当插件为空时直接返回
                if (descriptor == null)
                    return;

                //移除对应插件
                _installedPlugins.Remove(descriptor);

                //将插件添加到未安装插件列表
                _unInstalledPlugins.Add(descriptor);

                //将卸载的插件从安装的插件列表中移除
                _installedPluginSystemNames.Remove(descriptor.SystemName);
                SaveInstalledPluginSystemNameList(_installedPluginSystemNames);
            }
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            XmlHelper.SerializeToXml(_installedPluginSystemNames, WebHelper.MapPath(_installedPluginsPath));
        }

        /// <summary>
        /// 获得安装的插件系统名称列表
        /// </summary>
        private static List<string> GetInstalledPluginSystemNameList()
        {
            return (List<string>)XmlHelper.DeserializeFromXML(typeof(List<string>), WebHelper.MapPath(_installedPluginsPath));
        }

        /// <summary>
        /// 获得全部插件
        /// </summary>
        /// <param name="pluginFolder">插件目录</param>
        /// <returns></returns>
        private static List<KeyValuePair<FileInfo, PluginDescriptor>> GetAllPlugins(DirectoryInfo pluginFolder)
        {
            List<KeyValuePair<FileInfo, PluginDescriptor>> plugins = new List<KeyValuePair<FileInfo, PluginDescriptor>>();
            FileInfo[] pluginFiles = pluginFolder.GetFiles("plugin.config", SearchOption.AllDirectories);
            Type pluginType = typeof(PluginDescriptor);
            foreach (FileInfo file in pluginFiles)
            {
                PluginDescriptor p = (PluginDescriptor)XmlHelper.DeserializeFromXML(pluginType, file.FullName);
                plugins.Add(new KeyValuePair<FileInfo, PluginDescriptor>(file, p));
            }

            plugins.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return plugins;
        }
        /// <summary>
        /// 保存安装的插件系统名称列表
        /// </summary>
        /// <param name="installedPluginSystemNameList">安装的插件系统名称列表</param>
        private static void SaveInstalledPluginSystemNameList(List<string> installedPluginSystemNameList)
        {
            XmlHelper.SerializeToXml(installedPluginSystemNameList, WebHelper.MapPath(_installedPluginsPath));
        }

        /// <summary>
        /// 判断插件是否已经安装
        /// </summary>
        /// <param name="systemName">插件系统名称</param>
        /// <param name="installedPluginSystemNameList">安装的插件系统名称列表</param>
        /// <returns> </returns>
        private static bool IsInstalledlPlugin(string systemName, List<string> installedPluginSystemNameList)
        {
            foreach (string name in installedPluginSystemNameList)
            {
                if (name.Equals(systemName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        #endregion

        #region Utilities

        private static void SetPrivateEnvPath()
        {
            string dir = Environment.Is64BitProcess ? "amd64" : "x86";
            string envPath = String.Concat(Environment.GetEnvironmentVariable("PATH"), ";", Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, dir));
            Environment.SetEnvironmentVariable("PATH", envPath, EnvironmentVariableTarget.Process);
        }

        /// <summary>
        /// Indicates whether assembly file is already loaded
        /// </summary>
        /// <param name="fileInfo">File info</param>
        /// <returns>Result</returns>
        private static bool IsAlreadyLoaded(FileInfo fileInfo)
        {
            //do not compare the full assembly name, just filename
            try
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var a in assemblies)
                {
                    string assemblyName = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + exc);
            }
            return false;
        }

        /// <summary>
        /// 执行文件部署
        /// </summary>
        /// <param name="plug">Plugin file info</param>
        /// <returns>Reference to the shadow copied Assembly</returns>
        private static Assembly Probe(FileInfo plug)
        {
            if (plug.Directory == null || plug.Directory.Parent == null)
                throw new InvalidOperationException("The plugin directory for the " + plug.Name +
                                                    " file exists in a folder outside of the allowed SmartStore folder hierarchy");

            FileInfo shadowCopiedPlug;

            if (WebHelper.GetTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
            {
                // TODO: (mc) SMNET does not support Medium Trust, so this code is actually obsolete!

                // all plugins will need to be copied to ~/Plugins/bin/
                // this is aboslutely required because all of this relies on probingPaths being set statically in the web.config

                // were running in med trust, so copy to custom bin folder
                var shadowCopyPlugFolder = Directory.CreateDirectory(_shadowCopyPath);
                shadowCopiedPlug = InitializeMediumTrust(plug, shadowCopyPlugFolder);
            }
            else
            {
                var directory = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(plug.FullName + " to " + directory);	// codehint: sm-edit
                // we're running in full trust so copy to standard dynamic folder
                shadowCopiedPlug = InitializeFullTrust(plug, new DirectoryInfo(directory));
            }

            // we can now register the plugin definition
            var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlug.FullName));

            // add the reference to the build manager
            Debug.WriteLine("Adding to BuildManager: '{0}'", shadowCopiedAssembly.FullName);	// codehint: sm-edit
            BuildManager.AddReferencedAssembly(shadowCopiedAssembly);

            return shadowCopiedAssembly;
        }

        /// <summary>
        /// Used to initialize plugins when running in Full Trust
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeFullTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
            try
            {
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            catch (IOException)
            {
                Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                //this occurs when the files are locked,
                //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                try
                {
                    var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(shadowCopiedPlug.FullName, oldFile);
                }
                catch (IOException exc)
                {
                    throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                }
                //ok, we've made it this far, now retry the shadow copy
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            return shadowCopiedPlug;
        }

        /// <summary>
        /// Used to initialize plugins when running in Medium Trust
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeMediumTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));

            //check if a shadow copied file already exists and if it does, check if it's updated, if not don't copy
            if (shadowCopiedPlug.Exists)
            {
                //it's better to use LastWriteTimeUTC, but not all file systems have this property
                //maybe it is better to compare file hash?
                var areFilesIdentical = shadowCopiedPlug.CreationTimeUtc.Ticks >= plug.CreationTimeUtc.Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("Not copying; files appear identical: '{0}'", shadowCopiedPlug.Name);
                    shouldCopy = false;
                }
                else
                {
                    //delete an existing file
                    Debug.WriteLine("New plugin found; Deleting the old file: '{0}'", shadowCopiedPlug.Name);
                    File.Delete(shadowCopiedPlug.FullName);
                }
            }

            if (shouldCopy)
            {
                try
                {
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
                catch (IOException)
                {
                    Debug.WriteLine(shadowCopiedPlug.FullName + " is locked, attempting to rename");
                    //this occurs when the files are locked,
                    //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                    //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                    try
                    {
                        var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(shadowCopiedPlug.FullName, oldFile);
                    }
                    catch (IOException exc)
                    {
                        throw new IOException(shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin", exc);
                    }
                    //ok, we've made it this far, now retry the shadow copy
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
            }

            return shadowCopiedPlug;
        }

        /// <summary>
        /// Determines if the folder is a bin plugin folder for a package
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder == null) return false;
            if (folder.Parent == null) return false;
            if (!folder.Parent.Name.Equals("Plugins", StringComparison.InvariantCultureIgnoreCase)) return false;
            return true;
        }

        #endregion
    }
}
