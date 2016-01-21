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
        public PagedList<RoleInfo> Query(RoleQueryContext q)
        {
            return _roleRepository.Query(q);
        }

        public PagedList<RoleInfo> Query(Func<RoleQueryContext, RoleQueryContext> container)
        {
            RoleQueryContext q = container(new RoleQueryContext());

            return _roleRepository.Query(q);
        }
        public List<RoleInfo> GetAll(RoleQueryContext q)
        {
            return _roleRepository.GetAll(q);
        }
        public List<RoleInfo> GetAll(Func<RoleQueryContext, RoleQueryContext> container)
        {
            RoleQueryContext q = container(new RoleQueryContext());
            return _roleRepository.GetAll(q);
        }
    }
}
