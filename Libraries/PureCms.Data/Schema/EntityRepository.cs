using PetaPoco;
using PureCms.Core;
using PureCms.Core.Context;
using PureCms.Core.Domain.Logging;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PureCms.Data.Schema
{
    public class EntityRepository : IEntityRepository
    {
        private static readonly DataRepository<EntityInfo> _repository = new DataRepository<EntityInfo>();

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
        public EntityRepository()
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
        public bool Create(EntityInfo entity)
        {
            _repository.BeginTransaction();
            var flag = false;
            try
            {
                entity.CreatedOn = DateTime.Now;
                flag = _repository.CreateObject(entity);
                //自动创建的字段
                List<AttributeInfo> attrList = new List<AttributeInfo>();
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = entity.Name + "Id", LocalizedName = entity.LocalizedName, AttributeTypeId = Guid.Parse(AttributeTypeIds.PRIMARYKEY), IsNullable = false, IsRequired = true, IsLoged = false, DefaultValue = "newid()" });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "Name", LocalizedName = "名称", AttributeTypeId = Guid.Parse(AttributeTypeIds.NVARCHAR), IsNullable = false, IsRequired = true, IsLoged = false });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "VersionNumber", LocalizedName = "版本号", AttributeTypeId = Guid.Parse(AttributeTypeIds.TIMESTAMP), IsNullable = false, IsRequired = true, IsLoged = false });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "CreatedOn", LocalizedName = "创建日期", AttributeTypeId = Guid.Parse(AttributeTypeIds.DATETIME), IsNullable = false, IsRequired = true, IsLoged = false, DefaultValue = "getdate()" });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "CreatedBy", LocalizedName = "创建者", AttributeTypeId = Guid.Parse(AttributeTypeIds.LOOKUP), IsNullable = false, IsRequired = true, IsLoged = false });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "ModifiedOn", LocalizedName = "修改日期", AttributeTypeId = Guid.Parse(AttributeTypeIds.DATETIME), IsNullable = false, IsRequired = true, IsLoged = false, DefaultValue = "getdate()" });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "ModifiedBy", LocalizedName = "修改者", AttributeTypeId = Guid.Parse(AttributeTypeIds.LOOKUP), IsNullable = false, IsRequired = true, IsLoged = false });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "OwnerId", LocalizedName = "所有者", AttributeTypeId = Guid.Parse(AttributeTypeIds.OWNER), IsNullable = false, IsRequired = true, IsLoged = false });
                attrList.Add(new AttributeInfo() { AttributeId = Guid.NewGuid(), EntityId = entity.EntityId, Name = "StateCode", LocalizedName = "状态", AttributeTypeId = Guid.Parse(AttributeTypeIds.STATE), IsNullable = false, IsRequired = true, IsLoged = false });
                //创建数据库表
                Sql s = Sql.Builder.Append(string.Format("CREATE TABLE [dbo].[{0}]", entity.Name))
                .Append("(")
                .Append(string.Format("[{0}Id] [uniqueidentifier] NOT NULL,", entity.Name))
                .Append("[Name] [nvarchar(300)] NULL,")
                .Append("[VersionNumber] [timestamp] NOT NULL,")
                .Append("[CreatedOn] [datetime] NOT NULL,")
                .Append("[CreatedBy] [uniqueidentifier] NOT NULL,")
                .Append("[ModifiedOn] [datetime] NULL,")
                .Append("[ModifiedBy] [uniqueidentifier] NULL,")
                .Append("[OwnerId] [uniqueidentifier] NOT NULL,")
                .Append("[StateCode] [int] NOT NULL,")
                .Append(string.Format("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED", entity.Name))
                .Append("(")
                .Append(string.Format("[{0}Id] ASC", entity.Name))
                .Append(")WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]")
                .Append(") ON [PRIMARY]");
                _repository.Execute(s);
                //插入自动创建的字段，这里失败的话，事务不会回滚
                flag = new DataRepository<AttributeInfo>(_repository.GetConnection()).CreateMany(attrList);
                //创建关系

                //创建视图
                Sql sview = Sql.Builder.Append(string.Format("CREATE VIEW [dbo].[{0}View] AS", entity.Name))
                    .Append("SELECT ");
                //查询所有字段
                foreach (var item in attrList)
                {
                    sview.Append(string.Format("[{0}].[{1}],", entity.Name, item.Name));
                }
                //link attributes
                sview.Append("[lk_createdby].[Name] AS CreatedByName,")
                    .Append("[lk_modifiedby].[Name] AS ModifiedByName,")
                    .Append("[lk_owner].[Name] AS OwnerIdName");
                //tables
                sview.Append(string.Format("FROM [{0}] WITH(NOLOCK)", entity.Name))
                .Append("LEFT JOIN [Users] AS [lk_createdby] WITH(NOLOCK) ON [{0}].[CreatedBy]=[lk_createdby].[UserId]", entity.Name)
                .Append("LEFT JOIN [Users] AS [lk_modifiedby] WITH(NOLOCK) ON [{0}].[ModifiedBy]=[lk_modifiedby].[UserId]", entity.Name)
                .Append("LEFT JOIN [Users] AS [lk_owner] WITH(NOLOCK) ON [{0}].[OwnerId]=[lk_owner].[UserId]", entity.Name);
                _repository.Execute(s);

                if (flag)
                {
                    _repository.CompleteTransaction();
                }
                else
                {
                    _repository.RollBackTransaction();
                }
            }
            catch (Exception e)
            {
                _repository.RollBackTransaction();
                throw new PureCmsException(e.Message);
            }
            return flag;
        }
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(EntityInfo entity)
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
            var flag = false;
            _repository.BeginTransaction();
            try
            {
                //删除物理表
                var entity = this.FindById(id);
                Sql s = Sql.Builder.Append(string.Format("DROP TABLE {0}", entity.Name));
                _repository.Execute(s);
                flag = _repository.Delete(id);

                if (flag)
                {
                    _repository.CompleteTransaction();
                }
                else
                {
                    _repository.RollBackTransaction();
                }
            }
            catch (Exception e)
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
        public bool Update(UpdateContext<EntityInfo> context)
        {
            return _repository.Update(context);
        }
        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public long Count(QueryDescriptor<EntityInfo> q)
        {
            return _repository.Count(q);
        }
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="q">上下文</param>
        /// <returns></returns>
        public PagedList<EntityInfo> QueryPaged(QueryDescriptor<EntityInfo> q)
        {
            return _repository.QueryPaged(q);
        }
        public List<EntityInfo> Query(QueryDescriptor<EntityInfo> q)
        {
            return _repository.Query(q);
        }
        /// <summary>
        /// 查询一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityInfo FindById(Guid id)
        {
            return _repository.FindById(id);
        }
        #endregion
    }
}
