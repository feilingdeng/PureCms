using PureCms.Core.Caching;
using PureCms.Core.Domain.Schema;
using System.Collections.Generic;

namespace PureCms.Data.Schema
{
    public class ColumnReader
    {
        public const string CACHE_KEY = "$columns$";
        private static ICache _cache = new AspNetCache();
        private static readonly DataRepository<ColumnInfo> _repository = new DataRepository<ColumnInfo>();
        /// <summary>
        /// 实体元数据
        /// </summary>
        private PetaPoco.Database.PocoData MetaData
        {
            get
            {
                return _repository.MetaData;
            }
        }

        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }

        public ColumnReader()
        {

        }
        public static List<ColumnInfo> Columns
        {
            get
            {
                if (_cache.Contains(CACHE_KEY))
                {
                    return _cache.Get(CACHE_KEY) as List<ColumnInfo>;
                }
                var result = _repository.FindAll();
                _cache.Set(CACHE_KEY, result, null);
                return result;
            }
        }

        public ColumnInfo FindByTableName(string tableName)
        {
            return _repository.Find(w => w.TableName == tableName);
        }
        public ColumnInfo FindByColumnName(string columnName)
        {
            return _repository.Find(w => w.ColumnName == columnName);
        }
    }
}
