using System;
using PetaPoco;

namespace PureCms.Core.Domain.Schema
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

        public Guid EntityId { get; set; }

        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "ObjectTypeCode")]
        public int ObjectTypeCode { get; set; }
    }
}
