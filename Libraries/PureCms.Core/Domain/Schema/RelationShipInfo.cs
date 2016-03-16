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
        [ResultColumn]
        public string ReferencedEntityName { get; set; }
        [ResultColumn]
        public string ReferencedEntityLocalizedName { get; set; }
        [ResultColumn]
        public string ReferencedAttributeName { get; set; }
        [ResultColumn]
        public string ReferencedAttributeTypeName { get; set; }
        [ResultColumn]
        public string ReferencedAttributeLocalizedName { get; set; }
        [ResultColumn]
        public string ReferencingEntityName { get; set; }
        [ResultColumn]
        public string ReferencingAttributeName { get; set; }
        [ResultColumn]
        public string ReferencingEntityLocalizedName { get; set; }
        [ResultColumn]
        public string ReferencingAttributeLocalizedName { get; set; }
        [ResultColumn]
        public string ReferencingAttributeTypeName { get; set; }
        [Ignore]
        public new DateTime CreatedOn { get; set; }
    }
}
