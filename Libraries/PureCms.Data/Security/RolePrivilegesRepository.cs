using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System.Collections.Generic;

namespace PureCms.Data.Security
{
    public class RolePrivilegesRepository : IRolePrivilegesRepository
    {
        private static readonly DataRepository<RolePrivilegesInfo> _repository = new DataRepository<RolePrivilegesInfo>();

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
        public RolePrivilegesRepository()
        {
        }
        #region Implements
        public int Create(RolePrivilegesInfo entity)
        {
            return _repository.Create(entity);
        }

        public bool CreateMany(List<RolePrivilegesInfo> entities)
        {
            return _repository.CreateMany(entities);
        }

        public bool Update(RolePrivilegesInfo entity)
        {
            return _repository.Update(entity);
        }

        public bool DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        public bool DeleteByRoleId(int roleId)
        {
            return _repository.Delete(n=>n.RoleId == roleId);
        }
        public long Count(QueryDescriptor<RolePrivilegesInfo> q)
        {
            return _repository.Count(q);
        }
        public PagedList<RolePrivilegesInfo> QueryPaged(QueryDescriptor<RolePrivilegesInfo> q)
        {
            return _repository.QueryPaged(q);
        }

        public RolePrivilegesInfo FindById(int id)
        {
            return _repository.FindById(id);
        }
        public List<RolePrivilegesInfo> Query(QueryDescriptor<RolePrivilegesInfo> q)
        {
            return _repository.Query(q);
        }

        #endregion
    }
}
