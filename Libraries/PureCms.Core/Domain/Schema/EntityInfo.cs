using PetaPoco;
using System;

namespace PureCms.Core.Domain.Schema
{
    [TableName("Entity")]
    [PrimaryKey("EntityId", autoIncrement = false)]
    public class EntityInfo : BaseEntity
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public int ObjectTypeCode { get; set; }
        public bool IsLoged { get; set; }
        public bool IsCustomizable { get; set; }
        public string LocalizedName { get; set; }
        //public byte[] VersionNumber { get; set; }
        public Guid CreatedBy { get; set; }

        public bool IsAuthorization { get; set; }
    }
}
