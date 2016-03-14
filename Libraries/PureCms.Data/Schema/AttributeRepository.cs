using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PureCms.Data.Schema
{
    public class AttributeRepository : IAttributeRepository
    {
        private static readonly DataRepository<AttributeInfo> _repository = new DataRepository<AttributeInfo>();

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
        public AttributeRepository()
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
        public bool Create(AttributeInfo entity)
        {
            var flag = false;
            _repository.BeginTransaction();
            try {
                flag = _repository.CreateObject(entity);
                //新建字段
                Sql s = Sql.Builder.Append(string.Format("ALTER TABLE {0} ADD {1} {2}", entity.EntityName, entity.Name, GetDbType(entity)));
                _repository.Execute(s);
                _repository.CompleteTransaction();
            }
            catch(Exception e)
            {
                _repository.RollBackTransaction();
                throw new PureCmsException(e.Message);
            }
            return flag;
        }
        /// <summary>
        /// 指创建记录
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool CreateMany(List<AttributeInfo> entities)
        {
            return _repository.CreateMany(entities);
        }
        private string GetDbType(AttributeInfo attr)
        {
            string result = string.Empty;
            switch (attr.AttributeTypeId.ToString())
            {
                case AttributeTypeIds.UNIQUEIDENTIFIER:
                    result = "uniqueidentifier";
                    break;
                case AttributeTypeIds.BIGINT:
                    result = "bigint";
                    break;
                case AttributeTypeIds.BIT:
                    result = "bit";
                    break;
                case AttributeTypeIds.DATETIME:
                    result = "datetime";
                    break;
                case AttributeTypeIds.FLOAT:
                    result = "decimal(23,10)";
                    break;
                case AttributeTypeIds.INT:
                    result = "int";
                    break;
                case AttributeTypeIds.MONEY:
                    result = "money";
                    break;
                case AttributeTypeIds.NVARCHAR:
                    result = "nvarchar("+attr.MaxLength+")";
                    break;
                case AttributeTypeIds.NTEXT:
                    result = "ntext";
                    break;
                case AttributeTypeIds.VARCHAR:
                    result = "varchar(" + attr.MaxLength + ")";
                    break;
                case AttributeTypeIds.CHAR:
                    result = "char(" + attr.MaxLength + ")";
                    break;
                case AttributeTypeIds.NCHAR:
                    result = "nchar(" + attr.MaxLength + ")";
                    break;
                case AttributeTypeIds.TIMESTAMP:
                    result = "timestamp";
                    break;
            }
            return result;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(AttributeInfo entity)
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
            AttributeInfo entity = this.FindById(id);

            Sql s = Sql.Builder.Append(string.Format("ALTER TABLE {0} DROP COLUMN {1}", entity.EntityName, entity.Name));
            //删除对应的非公共optoionset，对应的视图、表单将在对应的加载方法里面处理
            if (entity.OptionSetId.HasValue)
            {
                s.Append(";DELETE OptionSet WHERE OptionSetId='" + entity.OptionSetId.Value + "' AND IsPublic=0");
            }
            _repository.BeginTransaction();
            var flag = false;
            try {
                flag = _repository.Delete(id);
                if (flag)
                {
                    _repository.Execute(s);
                    _repository.CompleteTransaction();
                }
                else
                {
                    _repository.RollBackTransaction();
                }
            }catch(Exception e)
            {
                _repository.RollBackTransaction();
                throw new PureCmsException(e.Message);
            }
            return flag;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteById(List<Guid> ids)
        {
            var flag = false;
            foreach (var id in ids)
            {
                flag = this.DeleteById(id);
            }
            return flag;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public bool Update(UpdateContext<AttributeInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<AttributeInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<AttributeInfo> QueryPaged(QueryDescriptor<AttributeInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<AttributeInfo> Query(QueryDescriptor<AttributeInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttributeInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttributeInfo Find(Guid entityId, string name)
        {
            return _repository.Find(x=>x.EntityId == entityId && x.Name == name);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public AttributeInfo Find(Expression<Func<AttributeInfo, bool>> predicate)
        {
            return _repository.Find(predicate);
        }
        #endregion
    }
}
