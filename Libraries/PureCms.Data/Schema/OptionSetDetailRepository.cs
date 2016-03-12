using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Data.Schema
{
    public class OptionSetDetailRepository : IOptionSetDetailRepository
    {
        private static readonly DataRepository<OptionSetDetailInfo> _repository = new DataRepository<OptionSetDetailInfo>();

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
        public OptionSetDetailRepository()
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
        public bool Create(OptionSetDetailInfo entity)
        {
            return _repository.CreateObject(entity);
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(OptionSetDetailInfo entity)
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
        public bool Update(UpdateContext<OptionSetDetailInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<OptionSetDetailInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<OptionSetDetailInfo> QueryPaged(QueryDescriptor<OptionSetDetailInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<OptionSetDetailInfo> Query(QueryDescriptor<OptionSetDetailInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OptionSetDetailInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        #endregion
    }
}
