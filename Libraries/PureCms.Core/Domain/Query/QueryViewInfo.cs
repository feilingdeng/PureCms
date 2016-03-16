using System;
using PetaPoco;
using PureCms.Core.Domain.Schema;

namespace PureCms.Core.Domain.Query
{
    [TableName("QueryView")]
    [PrimaryKey("QueryViewId", autoIncrement = false)]
    public class QueryViewInfo : BaseEntity
    {
        public Guid QueryViewId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public string FetchConfig { get; set; }
        public string LayoutConfig { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? OwnerId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSimpleFilter { get; set; }

        public Guid EntityId { get; set; }
        public string SqlString { get; set; }

        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "EntityId", LinkToFieldName = "EntityId")]
        public int ObjectTypeCode { get; set; }
        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "EntityId", LinkToFieldName = "EntityId", TargetFieldName = "Name")]
        public string EntityName { get; set; }
    }
}
