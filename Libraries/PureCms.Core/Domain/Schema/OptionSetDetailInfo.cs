using PetaPoco;
using System;

namespace PureCms.Core.Domain.Schema
{
    [TableName("OptionSetDetail")]
    [PrimaryKey("OptionSetDetailId", autoIncrement = false)]
    public class OptionSetDetailInfo : BaseEntity
    {
        public Guid OptionSetDetailId { get; set; }
        public Guid OptionSetId { get; set; }
        //public byte[] VersionNumber { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int DisplayOrder { get; set; }
        [Ignore]
        public new DateTime CreatedOn { get; set; }
    }
}
