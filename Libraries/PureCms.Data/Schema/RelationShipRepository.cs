using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Data.Schema
{
    public class RelationShipRepository : IRelationShipRepository
    {
        private static readonly DataRepository<RelationShipInfo> _repository = new DataRepository<RelationShipInfo>();

        private PetaPoco.Database.PocoData _metaData;
        /// <summary>
        /// 实体元数据
        /// </summary>
        private PetaPoco.Database.PocoData MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                _metaData = value;
            }
        }
        public RelationShipRepository()
        {
            _metaData = _repository.MetaData;
            _tableName = MetaData.TableInfo.TableName;
        }
        private string _tableName;
        /// <summary>
        /// 实体表名
        /// </summary>
        private string TableName
        {
            get
            {
                return _tableName;
            }
            set {
                _tableName = value;
                _metaData.TableInfo.TableName = value;
            }
        }
        #region implements
        /// <summary>
        /// 创建记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Create(RelationShipInfo entity)
        {
            return _repository.Create(entity);
        }
        /// <summary>
        /// 指创建记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool CreateMany(List<RelationShipInfo> entities)
        {
            return _repository.CreateMany(entities);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(RelationShipInfo entity)
        {
            return _repository.Update(entity);
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(Guid id)
        {
            return _repository.Delete(id);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteById(List<Guid> ids)
        {
            return _repository.Delete(ids);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Update(UpdateContext<RelationShipInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<RelationShipInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<RelationShipInfo> QueryPaged(QueryDescriptor<RelationShipInfo> q)
        {
            //this.TableName = "RelationShipView";
            return _repository.QueryPaged(q);
        }
        public List<RelationShipInfo> Query(QueryDescriptor<RelationShipInfo> q)
        {
            //this.TableName = "RelationShipView";
            return _repository.Query(q);
        }
        public List<RelationShipInfo> QueryByEntityId(Guid referencingEntityId)
        {
            string sql = "SELECT * FROM RelationShipView WHERE [ReferencingEntityId]=@0";
            return _repository.ExecuteQuery(sql, referencingEntityId);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RelationShipInfo FindById(Guid id)
        {
            //this.TableName = "RelationShipView";
            return _repository.FindById(id);
        }
        #endregion
    }
}
