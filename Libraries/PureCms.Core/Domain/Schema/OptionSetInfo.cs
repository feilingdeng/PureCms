using PetaPoco;
using System;

namespace PureCms.Core.Domain.Schema
{
    [TableName("OptionSet")]
    [PrimaryKey("OptionSetId", autoIncrement = false)]
    public class OptionSetInfo : BaseEntity
    {
        public Guid OptionSetId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        //public byte[] VersionNumber { get; set; }
    }
}
