using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Data;
using PureCms.Core.Domain.Cms;
using PureCms.Core.Cms;
using System.Collections.Generic;
using System.Linq;

namespace PureCms.Data.Cms
{
    public class ChannelRepository : IChannelRepository
    {
        private static readonly DataRepository<ChannelInfo> _repository = new DataRepository<ChannelInfo>();

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

        public ChannelRepository()
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
        public int Create(ChannelInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(ChannelInfo entity)
        {
            return _repository.Update(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="q">过滤条件</param>
        /// <returns></returns>
        public bool Update(UpdateContext<ChannelInfo> q)
        {
            return _repository.Update(q);
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
        public long Count(QueryDescriptor<ChannelInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<ChannelInfo> Query(QueryDescriptor<ChannelInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<ChannelInfo> GetAll(QueryDescriptor<ChannelInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChannelInfo GetById(int id)
        {
            return _repository.FindById(id);
        }

        public int MoveNode(int moveid, int targetid, int parentid, string position)
        {
            int result = 0;
            var moveNode = GetById(moveid);
            var targetNode = GetById(targetid);
            Sql s = Sql.Builder;
            switch (position)
            {
                case "after":
                    if (moveNode.ParentChannelId == targetNode.ParentChannelId)
                    {
                        //移动节点序号等于目标节点的序号+1
                        s.Append("UPDATE Channels SET DisplayOrder="+(targetNode.DisplayOrder+1)+" WHERE ChannelId=@0;", moveid);
                    }
                    break;
                case "inside":
                    if (moveNode.ParentChannelId == targetid)
                    {
                        //移动节点排第一，其它的排序+1
                        s.Append("UPDATE Channels SET DisplayOrder=0 WHERE ChannelId=@0;", moveid);
                    }
                    break;
                default:
                    result = -1;
                    break;
            }
            if (s.SQL.IsNotEmpty())
            {
                //重新排序
                s.Append("SELECT IDENTITY(INT,1,1) AS displayorder,ChannelId*1 AS ChannelId INTO #tmp");
                s.Append("FROM [Privileges] WHERE ParentChannelId=@0 ORDER BY displayorder", moveNode.ParentChannelId);
                s.Append("UPDATE a SET DisplayOrder = b.displayorder FROM [Channels] a");
                s.Append("INNER JOIN #tmp b ON a.ChannelId=b.ChannelId");
                s.Append("DROP TABLE #tmp");
                _repository.Execute(s);
                result = 1;
            }
            return result;
        }
        #endregion
    }
}
