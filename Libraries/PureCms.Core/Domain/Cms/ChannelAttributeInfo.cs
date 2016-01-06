using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("ChannelAttributes")]
    [PrimaryKey("ChannelAttributeId", autoIncrement = true)]
    public class ChannelAttributeInfo : BaseEntity
    {
        public int ChannelAttributeId { get; set; }
        public int CustomAttributeId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
