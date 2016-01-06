using PetaPoco;
using PureCms.Core.Configuration;
using PureCms.Core.Data;
using PureCms.Core.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PureCms;
using PureCms.Utilities;

namespace PureCms.Data.Configuration
{
    public class SettingRepository : ISettingRepository
    {
        MsSqlProvider<Setting> _settingRepository = new MsSqlProvider<Setting>();



        public Setting GetSettingById(int settingId)
        {
            throw new NotImplementedException();
        }

        public T GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            throw new NotImplementedException();
        }

        public IList<Setting> GetAllSettings()
        {
            throw new NotImplementedException();
        }

        public bool SettingExists<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public T LoadSetting<T>() where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T>(T settings) where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void UpdateSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void InsertSetting(Setting setting, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void UpdateSetting(Setting setting, bool clearCache = true)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(Setting setting)
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T>() where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettingRepository, new()
        {
            throw new NotImplementedException();
        }

        public void DeleteSetting(string key)
        {
            throw new NotImplementedException();
        }

        public int DeleteSettings(string rootKey)
        {
            throw new NotImplementedException();
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }
    }
}
