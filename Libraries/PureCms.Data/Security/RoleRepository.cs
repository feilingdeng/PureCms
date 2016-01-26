using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System.Collections.Generic;

namespace PureCms.Data.Security
{
    public class RoleRepository : IRoleRepository
    {
        private static readonly DataRepository<RoleInfo> _repository = new DataRepository<RoleInfo>();

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
        public RoleRepository()
        {
        }

        #region Implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(RoleInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(RoleInfo entity)
        {
            return _repository.Update(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="context">过滤条件</param>
        /// <returns></returns>
        public bool Update(UpdateContext<RoleInfo> context)
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
        public long Count(QueryDescriptor<RoleInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<RoleInfo> QueryPaged(QueryDescriptor<RoleInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<RoleInfo> Query(QueryDescriptor<RoleInfo> q)
        {
            return _repository.Query(q);
        }

        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RoleInfo FindById(int id)
        {
            return _repository.FindById(id);
        }
        #endregion
    }
}
