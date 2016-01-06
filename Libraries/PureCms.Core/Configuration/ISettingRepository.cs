using PureCms.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Configuration
{
    /// <summary>
    /// Setting service interface
    /// </summary>
    public partial interface ISettingRepository
    {
        /// <summary>
        /// Gets a setting by identifier
        /// </summary>
        /// <param name="settingId">Setting identifier</param>
        /// <returns>Setting</returns>
        Setting GetSettingById(int settingId);

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="defaultValue">Default value</param>/// <returns>Setting value</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Settings</returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// Determines whether a setting exists
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>true -setting exists; false - does not exist</returns>
        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettingRepository, new();

        /// <summary>
        /// Load settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        T LoadSetting<T>() where T : ISettingRepository, new();

        /// <summary>
        /// Set setting value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SetSetting<T>(string key, T value, bool clearCache = true);

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="settings">Setting instance</param>
        /// <param name="storeId">Store identifier</param>
        void SaveSetting<T>(T settings) where T : ISettingRepository, new();

        /// <summary>
        /// Save settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="storeId">Store ID</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettingRepository, new();

        void UpdateSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettingRepository, new();

        void InsertSetting(Setting setting, bool clearCache = true);

        void UpdateSetting(Setting setting, bool clearCache = true);

        /// <summary>
        /// Deletes a setting
        /// </summary>
        /// <param name="setting">Setting</param>
        void DeleteSetting(Setting setting);

        /// <summary>
        /// Delete all settings
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        void DeleteSetting<T>() where T : ISettingRepository, new();

        /// <summary>
        /// Delete settings object
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettingRepository, new();

        /// <remarks>codehint: sm-add</remarks>
        void DeleteSetting(string key);

        /// <summary>
        /// Deletes all settings with its key beginning with rootKey.
        /// </summary>
        /// <returns>Number of deleted settings</returns>
        int DeleteSettings(string rootKey);

        /// <summary>
        /// Clear cache
        /// </summary>
        void ClearCache();
    }
}
