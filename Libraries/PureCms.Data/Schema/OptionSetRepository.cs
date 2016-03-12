using PureCms.Core.Context;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Data.Schema
{
    public class OptionSetRepository : IOptionSetRepository
    {
        private static readonly DataRepository<OptionSetInfo> _repository = new DataRepository<OptionSetInfo>();

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
        public OptionSetRepository()
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
        public bool Create(OptionSetInfo entity)
        {
            return _repository.CreateObject(entity);
        }
        public bool Create(OptionSetInfo entity, List<OptionSetDetailInfo> details)
        {
            var flag = false;
            try {
                _repository.BeginTransaction();
                flag = _repository.CreateObject(entity);

                var _detailRepository = new DataRepository<OptionSetDetailInfo>();
                flag = _detailRepository.CreateMany(details);
                _repository.CompleteTransaction();
            }
            catch(Exception e)
            {
                _repository.RollBackTransaction();
            }

            return flag;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(OptionSetInfo entity)
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
        public bool Update(UpdateContext<OptionSetInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<OptionSetInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<OptionSetInfo> QueryPaged(QueryDescriptor<OptionSetInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<OptionSetInfo> Query(QueryDescriptor<OptionSetInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OptionSetInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        #endregion
    }
}
