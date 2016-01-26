using PetaPoco;
using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using PureCms.Core.Security;
using System.Collections.Generic;

namespace PureCms.Data.Security
{
    public class PrivilegeRepository : IPrivilegeRepository
    {
        private static readonly DataRepository<PrivilegeInfo> _repository = new DataRepository<PrivilegeInfo>();

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
        private string TableName{
            get
            {
                return MetaData.TableInfo.TableName;
            }
        }
        public PrivilegeRepository()
        {
            //PetaPoco.Database.Mapper = new ColumnMapper();
        }
        #region Implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(PrivilegeInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(PrivilegeInfo entity)
        {
            return _repository.Update(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="sets">需要设置的字段和值</param>
        /// <param name="context">过滤条件</param>
        /// <returns></returns>
        public bool Update(UpdateContext<PrivilegeInfo> context)
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
        public long Count(QueryDescriptor<PrivilegeInfo> q)
        {
            return _repository.Count(q);
        }


        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="moveid"></param>
        /// <param name="targetid"></param>
        /// <param name="parentid"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public int MoveNode(int moveid, int targetid, int parentid, string position)
        {
            int result = 0;
            var moveNode = FindById(moveid);
            var targetNode = FindById(targetid);
            Sql s = Sql.Builder;
            switch (position)
            {
                case "after":
                    if (moveNode.ParentPrivilegeId == targetNode.ParentPrivilegeId)
                    {
                        //移动节点序号等于目标节点的序号+1
                        s.Append("UPDATE Privileges SET DisplayOrder=" + (targetNode.DisplayOrder + 1) + " WHERE PrivilegeId=@0;", moveid);
                    }
                    break;
                case "inside":
                    if (moveNode.ParentPrivilegeId == targetid)
                    {
                        //移动节点排第一，其它的排序+1
                        s.Append("UPDATE Privileges SET DisplayOrder=0 WHERE PrivilegeId=@0;", moveid);
                    }
                    break;
                default:
                    result = -1;
                    break;
            }
            if (s.SQL.IsNotEmpty())
            {
                //重新排序
                s.Append("SELECT IDENTITY(INT,1,1) AS displayorder,PrivilegeId*1 AS PrivilegeId INTO #tmp");
                s.Append("FROM [Privileges] WHERE ParentPrivilegeId=@0 ORDER BY displayorder", moveNode.ParentPrivilegeId);
                s.Append("UPDATE a SET DisplayOrder = b.displayorder FROM [Privileges] a");
                s.Append("INNER JOIN #tmp b ON a.PrivilegeId=b.PrivilegeId");
                s.Append("DROP TABLE #tmp");
                _repository.Execute(s);
                result = 1;
            }
            return result;
            //SqlParameter[] ps = new SqlParameter[]{
            //    new SqlParameter() { SqlDbType = SqlDbType.Int,Value = moveid }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.Int, Value = targetid }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.NVarChar,Value = position }
            //    ,new SqlParameter() { SqlDbType = SqlDbType.Int,Value = 3 }
            //    ,new SqlParameter() { Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int }
            //};

            //((Database)_repository.Client).Execute("EXEC [usp_Security_UpdPrivilegeNode] @0,@1,@2,@3,@4 OUTPUT", ps);

            //return (int)ps[4].Value;
        }
        public PagedList<PrivilegeInfo> QueryPaged(QueryDescriptor<PrivilegeInfo> q)
        {
            return _repository.QueryPaged(q);
        }

        public PrivilegeInfo FindById(int id)
        {
            return _repository.FindById(id);
        }
        public PrivilegeInfo Find(QueryDescriptor<PrivilegeInfo> q)
        {
            return _repository.Find(q);
        }
        public List<PrivilegeInfo> Query(QueryDescriptor<PrivilegeInfo> q)
        {
            return _repository.Query(q);
        }
        
        #endregion
    }

   
}
