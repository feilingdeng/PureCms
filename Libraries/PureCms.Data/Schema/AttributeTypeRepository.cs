using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Data.Schema
{
    public class AttributeTypeRepository : IAttributeTypeRepository
    {
        private static readonly DataRepository<AttributeTypeInfo> _repository = new DataRepository<AttributeTypeInfo>();

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
        public AttributeTypeRepository()
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
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<AttributeTypeInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<AttributeTypeInfo> QueryPaged(QueryDescriptor<AttributeTypeInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<AttributeTypeInfo> Query(QueryDescriptor<AttributeTypeInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AttributeTypeInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        public AttributeTypeInfo FindByName(string name)
        {
            return _repository.Find(x=>x.Name == name);
        }
        #endregion
    }
}
