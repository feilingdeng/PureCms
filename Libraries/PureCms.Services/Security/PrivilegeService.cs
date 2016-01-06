using PureCms.Core;
using PureCms.Core.Caching;
using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using PureCms.Data.Security;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PureCms.Services.Security
{
    public class PrivilegeService
    {
        private IPrivilegeRepository _repository = new PrivilegeRepository();
        private ICache _cache = new AspNetCache();
        public const string CACHE_KEY = "$Privileges$";

        public int Creat(PrivilegeInfo entity)
        {
            var parent = GetById(entity.ParentPrivilegeId);
            if (parent != null)
            {
                entity.Level = parent.Level + 1;
            }
            var count = _repository.Count(new PrivilegeQueryContext()
            {
                ParentPrivilegeId = entity.ParentPrivilegeId
            });
            entity.DisplayOrder = ((int)count + 1);
            if (entity.Level <= 0)
            {
                entity.Level = 1;
            }
            _cache.Remove(CACHE_KEY);
            return _repository.Create(entity);
        }
        public bool Update(PrivilegeInfo entity)
        {
            _cache.Remove(CACHE_KEY);
            return _repository.Update(entity);
        }

        public PrivilegeInfo GetById(int id)
        {
            return _repository.GetById(id);
        }
        public bool DeleteById(int id)
        {
            _cache.Remove(CACHE_KEY);
            return _repository.DeleteById(id);
        }

        public PagedList<PrivilegeInfo> Query(Func<PrivilegeQueryContext, PrivilegeQueryContext> container)
        {
            PrivilegeQueryContext q = container(new PrivilegeQueryContext());

            return _repository.Query(q);
        }
        public List<PrivilegeInfo> GetAll(Func<PrivilegeQueryContext, PrivilegeQueryContext> container)
        {
            PrivilegeQueryContext q = container(new PrivilegeQueryContext());
            return _repository.GetAll(q);
        }
        public List<PrivilegeInfo> GetAll()
        {
            List<PrivilegeInfo> result = null;
            if (_cache.Contains(CACHE_KEY))
            {
                result = _cache.Get(CACHE_KEY) as List<PrivilegeInfo>;
            }
            else
            {
                result = this.GetAll(n=>n.Sort(s=>s.SortDescending(f=>f.DisplayOrder)));
            }

            return result;
        }
        public PrivilegeInfo GetOne(Func<PrivilegeQueryContext, PrivilegeQueryContext> container)
        {
            PrivilegeQueryContext q = container(new PrivilegeQueryContext());
            return _repository.GetOne(q);
        }

        public int Move(int moveid, int targetid, int parentid, string position)
        {
            int result = _repository.MoveNode(moveid,targetid,parentid,position);
            return result;
        }

        public List<PrivilegeInfo> GetTreePath(string className, string methodName)
        {
            List<PrivilegeInfo> result = new List<PrivilegeInfo>();
            var all = this.GetAll();
            var current = all.Find(n => n.ClassName.ToLower() == className.ToLower() && n.MethodName.ToLower() == methodName.ToLower());
            if (null != current)
            {
                var flag = current.Level > 1;
                result.Add(current);
                int parentid = current.ParentPrivilegeId;
                while (flag)
                {
                    var parent = all.Find(n => n.PrivilegeId == parentid);
                    if (parent != null)
                    {
                        result.Add(parent);
                        parentid = parent.ParentPrivilegeId;
                        if (parent.Level <= 1)
                        {
                            flag = false;
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                result.Reverse();
            }

            return result;
        }

        #region json相关
        public string GetJsonData(Func<PrivilegeQueryContext, PrivilegeQueryContext> container, bool nameLower = true)
        {
            PrivilegeQueryContext q = container(new PrivilegeQueryContext());

            List<PrivilegeInfo> list = _repository.GetAll(q);
            string json = string.Empty;

            List<dynamic> dlist = BuildTree(list, 0);
            dynamic contact = new ExpandoObject();
            contact.label = "根节点";
            contact.id = 0;
            contact.children = dlist;

            List<dynamic> results = new List<dynamic>();
            results.Add(contact);

            json = results.SerializeToJson(nameLower);
            return json;
        }

        private List<dynamic> BuildTree(List<PrivilegeInfo> privilegeList, int parentId)
        {
            List<dynamic> dynamicList = new List<dynamic>();
            List<PrivilegeInfo> childList = privilegeList.Where(n => n.ParentPrivilegeId == parentId).OrderBy(n => n.DisplayOrder).ToList();
            if (childList != null && childList.Count > 0)
            {
                List<dynamic> ddList = new List<dynamic>();
                dynamic contact = new ExpandoObject();
                foreach (var item in childList)
                {
                    contact = new ExpandoObject();
                    contact.label = item.DisplayName;
                    contact.id = item.PrivilegeId;
                    if (privilegeList.Find(n => n.ParentPrivilegeId == item.PrivilegeId) != null)
                    {
                        ddList = BuildTree(privilegeList, item.PrivilegeId);
                        if (ddList.Count > 0)
                        {
                            contact.children = ddList;
                            ddList = new List<dynamic>();
                        }
                    }
                    dynamicList.Add(contact);
                }
            }
            return dynamicList;
        }
        #endregion
    }
}
