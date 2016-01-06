using PureCms.Core.Domain;
using System;

namespace PureCms.Core.Data
{
    public static class DataProviderFactory<T> where T : BaseEntity
    {
        /// <summary>
        /// 获取数据处理实例
        /// </summary>
        /// <returns></returns>
        public static IDataProvider<T> GetInstance(DataProvider provider)
        {
            IDataProvider<T> _repository = null;
            switch(provider)
            {
                case DataProvider.MSSQL:
                    _repository = new MsSqlProvider<T>();
                    break;
                case DataProvider.MYSQL:
                    _repository = new MySqlProvider<T>();
                    break;
                default:
                    break;
            }
            //Type target = typeof(T);
            //string repositoryName = target.Name.Replace("Info","Repository");
            //_repository = (IRepository<T>)Activator.CreateInstance(Type.GetType(string.Format("PureCms.{0}.Core.{1}, PureCms.{0}.Core", serviceName, repositoryName),
            //                                                                          false,
            //                                                                          true));
            if (null == _repository) throw new Exception("no repository provider is matched");
            return _repository;
        }
    }

    public enum DataProvider
    {
        MSSQL = 1
        ,MYSQL = 2
    }
}
