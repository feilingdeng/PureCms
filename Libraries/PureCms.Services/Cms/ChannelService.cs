using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using PureCms.Core.Utilities;
using PureCms.Data.Cms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using PureCms.Core.Caching;

namespace PureCms.Services.Cms
{
    public class ChannelService
    {
        private IChannelRepository _repository = new ChannelRepository();
        private ICache _cache = new AspNetCache();
        public const string CACHE_KEY = "$Channels$";


        public long Create(ChannelInfo entity)
        {
            var parent = GetById(entity.ParentChannelId);
            if (parent != null)
            {
                entity.Level = parent.Level + 1;
            }
            var q = new QueryDescriptor<ChannelInfo>();
            q.Where(n => n.ParentChannelId == entity.ParentChannelId);
            var count = _repository.Count(q);
            entity.DisplayOrder = ((int)count + 1);
            if (entity.Level <= 0)
            {
                entity.Level = 1;
            }
            return _repository.Create(entity);
        }
        public bool Update(ChannelInfo entity)
        {
            return _repository.Update(entity);
        }
        public bool Update(Func<UpdateContext<ChannelInfo>, UpdateContext<ChannelInfo>> context)
        {
            var ctx = context(new UpdateContext<ChannelInfo>());
            return _repository.Update(ctx);
        }

        public ChannelInfo GetById(int id)
        {
            return _repository.FindById(id);
        }
        public bool DeleteById(int id)
        {
            return _repository.DeleteById(id);
        }

        public PagedList<ChannelInfo> Query(Func<QueryDescriptor<ChannelInfo>, QueryDescriptor<ChannelInfo>> container)
        {
            QueryDescriptor<ChannelInfo> q = container(new QueryDescriptor<ChannelInfo>());

            return _repository.QueryPaged(q);
        }

        public List<ChannelInfo> GetAll(Func<QueryDescriptor<ChannelInfo>, QueryDescriptor<ChannelInfo>> container)
        {
            QueryDescriptor<ChannelInfo> q = container(new QueryDescriptor<ChannelInfo>());

            return _repository.Query(q);
        }

        public List<ChannelInfo> GetAll()
        {
            List<ChannelInfo> result = null;
            if (_cache.Contains(CACHE_KEY) != null)
            {
                result = _cache.Get(CACHE_KEY) as List<ChannelInfo>;
            }
            else
            {
                result = _repository.Query(null);
            }

            return result;
        }
        /// <summary>
        /// 排序，目前只支持同一节点下移动
        /// </summary>
        /// <param name="moveid"></param>
        /// <param name="targetid"></param>
        /// <param name="parentid"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public int Move(int moveid, int targetid, int parentid, string position)
        {
            int result = _repository.MoveNode(moveid, targetid, parentid, position);
            return result;
        }

        #region json相关
        /// <summary>
        /// 获取json数据
        /// </summary>
        /// <param name="container">查询上下文</param>
        /// <param name="nameLower">属性名称是否转换为小写</param>
        /// <param name="wrapRoot">是否包含于根节点</param>
        /// <returns></returns>
        public string GetJsonData(Func<QueryDescriptor<ChannelInfo>, QueryDescriptor<ChannelInfo>> container, bool nameLower = true, bool wrapRoot = true)
        {
            QueryDescriptor<ChannelInfo> q = container(new QueryDescriptor<ChannelInfo>());

            List<ChannelInfo> list = _repository.Query(q);
            string json = string.Empty;

            List<dynamic> results = new List<dynamic>();
            List<dynamic> dlist = BuildTree(list, 0);
            if (wrapRoot)
            {
                dynamic contact = new ExpandoObject();
                contact.label = "根节点";
                contact.id = 0;
                contact.children = dlist;
                results.Add(contact);
            }
            else
            {
                results = dlist;
            }

            json = results.SerializeToJson(nameLower);
            return json;
        }

        private List<dynamic> BuildTree(List<ChannelInfo> ChannelList, int parentId)
        {
            List<dynamic> dynamicList = new List<dynamic>();
            List<ChannelInfo> childList = ChannelList.Where(n => n.ParentChannelId == parentId).OrderBy(n => n.DisplayOrder).ToList();
            if (childList != null && childList.Count > 0)
            {
                List<dynamic> ddList = new List<dynamic>();
                dynamic contact = new ExpandoObject();
                foreach (var item in childList)
                {
                    contact = new ExpandoObject();
                    contact.label = item.Name;
                    contact.id = item.ChannelId;
                    if (ChannelList.Find(n => n.ParentChannelId == item.ChannelId) != null)
                    {
                        ddList = BuildTree(ChannelList, item.ChannelId);
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
