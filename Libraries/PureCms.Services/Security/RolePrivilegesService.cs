using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using PureCms.Core.Utilities;
using PureCms.Data.Security;
using PureCms.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PureCms.Services.Security
{
    public class RolePrivilegesService
    {
        IRolePrivilegesRepository _repository = new RolePrivilegesRepository();


        public int Creat(RolePrivilegesInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool CreatRolePrivileges(int roleId, List<int> privilegeId)
        {
            if (privilegeId.IsNotNullOrEmpty())
            {
                List<RolePrivilegesInfo> entities = new List<RolePrivilegesInfo>();
                foreach (var item in privilegeId)
                {
                    if (item > 0)
                    {
                        entities.Add(new RolePrivilegesInfo()
                        {
                            RoleId = roleId
                            ,
                            PrivilegeId = item
                        });
                    }
                }
                return _repository.CreateMany(entities);
            }
            return true;
        }
        public bool Update(RolePrivilegesInfo entity)
        {
            return _repository.Update(entity);
        }

        public RolePrivilegesInfo GetById(int id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }
        public bool DeleteByRoleId(int roleId)
        {
            return _repository.DeleteByRoleId(roleId);
        }

        public PagedList<RolePrivilegesInfo> Query(Func<QueryDescriptor<RolePrivilegesInfo>, QueryDescriptor<RolePrivilegesInfo>> container)
        {
            QueryDescriptor<RolePrivilegesInfo> q = container(new QueryDescriptor<RolePrivilegesInfo>());

            return _repository.QueryPaged(q);
        }
        public List<RolePrivilegesInfo> GetAll(Func<QueryDescriptor<RolePrivilegesInfo>, QueryDescriptor<RolePrivilegesInfo>> container)
        {
            QueryDescriptor<RolePrivilegesInfo> q = container(new QueryDescriptor<RolePrivilegesInfo>());
            return _repository.Query(q);
        }
    }
}
