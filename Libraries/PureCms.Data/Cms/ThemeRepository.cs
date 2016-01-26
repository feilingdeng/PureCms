using PureCms.Core.Context;
using PureCms.Core.Cms;
using System.Collections.Generic;
using PureCms.Core.Domain.Theme;

namespace PureCms.Data.Cms
{
    public class ThemeRepository : IThemeRepository
    {
        private static readonly DataRepository<ThemeInfo> _repository = new DataRepository<ThemeInfo>();

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

        public ThemeRepository()
        {
        }
        /// <summary>
        /// 实体表名
        /// </summary>
        private string TableName
        {
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        #region implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(ThemeInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(ThemeInfo entity)
        {
            return _repository.Update(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="context">过滤条件</param>
        /// <returns></returns>
        public bool Update(UpdateContext<ThemeInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {
            return _repository.Delete(id);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<ThemeInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 分页查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<ThemeInfo> QueryPaged(QueryDescriptor<ThemeInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<ThemeInfo> Query(QueryDescriptor<ThemeInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ThemeInfo FindById(int id)
        {
            return _repository.FindById(id);
        }
        #endregion
    }
}
