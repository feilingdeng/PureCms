using PetaPoco;
using System;

namespace PureCms.Core.Domain.Schema
{
    [TableName("Relationship")]
    [PrimaryKey("RelationshipId", autoIncrement = false)]
    public class RelationShipInfo : BaseEntity
    {
        public Guid RelationshipId { get; set; }
        public string Name { get; set; }
        public Guid ReferencingEntityId { get; set; }
        public Guid ReferencingAttributeId { get; set; }
        public Guid ReferencedEntityId { get; set; }
        public Guid ReferencedAttributeId { get; set; }
        public int RelationshipType { get; set; }
        [Ignore]
        public new DateTime CreatedOn { get; set; }
    }
}
