using PureCms.Core.Caching;
using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using PureCms.Data.Schema;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace PureCms.Services.Schema
{
    public class EntityService
    {
        IEntityRepository _repository = new EntityRepository();
        private ICache _cache = new AspNetCache();
        public const string CACHE_KEY = "$Entities$";

        public bool Create(EntityInfo entity)
        {
            return _repository.Create(entity);
        }
        public bool Update(EntityInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<EntityInfo>, UpdateContext<EntityInfo>> context)
        {
            var ctx = context(new UpdateContext<EntityInfo>());
            return _repository.Update(ctx);
        }

        public EntityInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(Guid id)
        {
            return _repository.DeleteById(id);
        }

        public bool DeleteById(List<Guid> ids)
        {
            return _repository.DeleteById(ids);
        }

        public PagedList<EntityInfo> QueryPaged(Func<QueryDescriptor<EntityInfo>, QueryDescriptor<EntityInfo>> container)
        {
            QueryDescriptor<EntityInfo> q = container(new QueryDescriptor<EntityInfo>());

            return _repository.QueryPaged(q);
        }
        public List<EntityInfo> Query(Func<QueryDescriptor<EntityInfo>, QueryDescriptor<EntityInfo>> container)
        {
            QueryDescriptor<EntityInfo> q = container(new QueryDescriptor<EntityInfo>());

            return _repository.Query(q);
        }

        #region json相关
        public string GetJsonData(Func<QueryDescriptor<EntityInfo>, QueryDescriptor<EntityInfo>> container, bool nameLower = true)
        {
            QueryDescriptor<EntityInfo> q = container(new QueryDescriptor<EntityInfo>());

            List<EntityInfo> list = _repository.Query(q);
            string json = string.Empty;

            List<dynamic> dlist = BuildTree(list);
            dynamic contact = new ExpandoObject();
            contact.label = "实体";
            contact.id = "";
            contact.entityname = "实体";
            contact.children = dlist;

            List<dynamic> results = new List<dynamic>();
            results.Add(contact);

            json = results.SerializeToJson(nameLower);
            return json;
        }

        private List<dynamic> BuildTree(List<EntityInfo> dataList)
        {
            List<dynamic> dynamicList = new List<dynamic>();
            dynamic contact = new ExpandoObject();
            foreach (var item in dataList)
            {
                contact = new ExpandoObject();
                contact.label = item.LocalizedName;
                contact.id = item.EntityId;
                contact.entityname = item.Name;
                dynamicList.Add(contact);
            }
            return dynamicList;
        }
        #endregion
    }
}
