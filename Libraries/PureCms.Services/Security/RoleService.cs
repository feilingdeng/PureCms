using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using PureCms.Core.Utilities;
using PureCms.Data.Security;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PureCms.Services.Security
{
    public class RoleService
    {
        IRoleRepository _roleRepository = new RoleRepository();


        public int Creat(RoleInfo entity)
        {
            return _roleRepository.Create(entity);
        }
        public bool Update(RoleInfo entity)
        {
            return _roleRepository.Update(entity);
        }

        public RoleInfo GetById(int id)
        {
            return _roleRepository.GetById(id);
        }
        public bool DeleteById(int id)
        {
            return _roleRepository.DeleteById(id);
        }
        public PagedList<RoleInfo> Query(QueryDescriptor<RoleInfo> q)
        {
            return _roleRepository.Query(q);
        }

        public PagedList<RoleInfo> Query(Func<QueryDescriptor<RoleInfo>, QueryDescriptor<RoleInfo>> container)
        {
            QueryDescriptor<RoleInfo> q = container(new QueryDescriptor<RoleInfo>());

            return _roleRepository.Query(q);
        }
        public List<RoleInfo> GetAll(QueryDescriptor<RoleInfo> q)
        {
            return _roleRepository.GetAll(q);
        }
        public List<RoleInfo> GetAll(Func<QueryDescriptor<RoleInfo>, QueryDescriptor<RoleInfo>> container)
        {
            QueryDescriptor<RoleInfo> q = container(new QueryDescriptor<RoleInfo>());
            return _roleRepository.GetAll(q);
        }
    }
}
