using PetaPoco;
using System;

namespace PureCms.Core.Domain.Schema
{
    [TableName("AttributeType")]
    [PrimaryKey("AttributeTypeId", autoIncrement = false)]
    public class AttributeTypeInfo : BaseEntity
    {
        public Guid AttributeTypeId { get; set; }
        public string Name { get; set; }
        public string XmlType { get; set; }

        [Ignore]
        public new DateTime CreatedOn { get; set; }
    }
}
