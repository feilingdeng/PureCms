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
        /// <summary>
        /// 实体元数据
        /// </summary>
        private static readonly PetaPoco.Database.PocoData MetaData = PetaPoco.Database.PocoData.ForType(typeof(ChannelInfo));
        private static readonly IDataProvider<ChannelInfo> _repository = DataProviderFactory<ChannelInfo>.GetInstance(DataProvider.MSSQL);

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
            var result = _repository.CreateAsync(entity);
            int id = int.Parse(result.Result.ToString());
            return id;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(ChannelInfo entity)
        {
            var result = _repository.UpdateAsync(entity);
            return result.Result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="q">过滤条件</param>
        /// <returns></returns>
        public bool Update(List<KeyValuePair<string, object>> sets, ChannelQueryContext q)
        {
            Guard.ArgumentNotNullOrEmpty<KeyValuePair<string, object>>(sets, "sets");

            Sql query = PetaPoco.Sql.Builder.Append("UPDATE " + TableName + " SET ");
            string optName = string.Empty;
            foreach (var item in sets)
            {
                query.Append(TableName + "." + item.Key + "=@0", item.Value);
            }
            query.Append(ParseWhereSql(q));
            int result = ((Database)_repository.Client).Execute(query);
            return result > 0;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(int id)
        {
            var result = _repository.DeleteAsync(id);
            return result.Result;
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(ChannelQueryContext q)
        {
            ExecuteContext<ChannelInfo> ctx = ParseQueryContext(q, null, true);
            var result = _repository.CountAsync(ctx);
            return result.Result;
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<ChannelInfo> Query(ChannelQueryContext q)
        {
            ExecuteContext<ChannelInfo> ctx = ParseQueryContext(q);
            var result = _repository.PagedAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                PagedList<ChannelInfo> list = new PagedList<ChannelInfo>()
                {
                    CurrentPage = pageDatas.CurrentPage
                    ,
                    ItemsPerPage = pageDatas.ItemsPerPage
                    ,
                    TotalItems = pageDatas.TotalItems
                    ,
                    TotalPages = pageDatas.TotalPages
                    ,
                    Items = pageDatas.Items
                };
                return list;
            }
            return null;
        }
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public List<ChannelInfo> GetAll(ChannelQueryContext q)
        {
            ExecuteContext<ChannelInfo> ctx = ParseQueryContext(q);
            var result = _repository.GetAllAsync(ctx);
            var pageDatas = result.Result;
            if (pageDatas != null)
            {
                return pageDatas.ToList();
            }
            return null;
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChannelInfo GetById(int id)
        {
            var result = _repository.GetByIdAsync(id);
            return result.Result;
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
                ((Database)_repository.Client).Execute(s);
                result = 1;
            }
            return result;
        }
        #endregion


        #region Utilities
        /// <summary>
        /// 根据上下文生成查询语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private Sql ParseSelectSql(ChannelQueryContext q, bool isCount = false)
        {
            var columns = ContextHelper.GetSelectColumns(MetaData, q.Columns, isCount);
            Sql query = PetaPoco.Sql.Builder.Append("SELECT " + columns + " FROM " + TableName);
            return query;
        }
        /// <summary>
        /// 根据上下文生成过滤条件语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <returns></returns>
        private Sql ParseWhereSql(ChannelQueryContext q, Sql otherCondition = null)
        {
            Sql query = PetaPoco.Sql.Builder;
            //过滤条件
            Sql filter = PetaPoco.Sql.Builder;
            string optName = string.Empty;

            if (q.ChannelId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ChannelId=@0", optName, TableName), q.ChannelId.Value);
                optName = " AND ";
            }
            if (q.IsEnabled.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsEnabled=@0", optName, TableName), q.IsEnabled.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.IsShow.HasValue)
            {
                filter.Append(string.Format("{0} {1}.IsShow=@0", optName, TableName), q.IsShow.Value == true ? 1 : 0);
                optName = " AND ";
            }
            if (q.Level.HasValue)
            {
                filter.Append(string.Format("{0} {1}.Level=@0", optName, TableName), q.Level.Value);
                optName = " AND ";
            }
            if (q.ContentType.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ContentType=@0", optName, TableName), q.ContentType.Value);
                optName = " AND ";
            }
            if (q.Name.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Name LIKE @0", optName, TableName), "%" + q.Name + "%");
                optName = " AND ";
            }
            if (q.SiteId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.SiteId=@0", optName, TableName), q.SiteId.Value);
                optName = " AND ";
            }
            if (q.Url.IsNotEmpty())
            {
                filter.Append(string.Format("{0} {1}.Url=@0", optName, TableName), q.Url);
                optName = " AND ";
            }
            if (q.ParentChannelId.HasValue)
            {
                filter.Append(string.Format("{0} {1}.ParentChannelId=@0", optName, TableName), q.ParentChannelId.Value);
                optName = " AND ";
            }
            if (filter.SQL.IsNotEmpty())
            {
                query.Append("WHERE ");
                query.Append(filter);
            }
            //其它条件
            if (otherCondition != null)
            {
                query.Append(optName);
                query.Append(otherCondition);
            }
            return query;
        }
        /// <summary>
        /// 根据上下文生成查询语句
        /// </summary>
        /// <param name="q">上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private Sql ParseQuerySql(ChannelQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            Sql query = PetaPoco.Sql.Builder.Append(ParseSelectSql(q, isCount))
                .Append(ParseWhereSql(q, otherCondition));
            //排序
            if (isCount == false)
            {
                query.Append(ContextHelper.GetOrderBy<ChannelInfo>(MetaData, q.SortingDescriptor));
            }

            return query;
        }
        /// <summary>
        /// 转换为数据库上下文
        /// </summary>
        /// <param name="q">实体上下文</param>
        /// <param name="otherCondition">其它附加过滤条件</param>
        /// <param name="isCount">是否统计数量</param>
        /// <returns></returns>
        private ExecuteContext<ChannelInfo> ParseQueryContext(ChannelQueryContext q, Sql otherCondition = null, bool isCount = false)
        {
            ExecuteContext<ChannelInfo> ctx = new ExecuteContext<ChannelInfo>()
            {
                ExecuteContainer = ParseQuerySql(q, otherCondition, isCount)
                ,
                PagingInfo = q.PagingDescriptor
                ,
                TopCount = q.TopCount
            };

            return ctx;
        }
        #endregion
    }
}
