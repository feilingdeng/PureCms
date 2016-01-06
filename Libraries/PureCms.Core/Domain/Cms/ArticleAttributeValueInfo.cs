using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("ArticleAttributeValues")]
    [PrimaryKey("ArticleAttributeValueId", autoIncrement = true)]
    public class ArticleAttributeValueInfo : BaseEntity
    {
        public int ArticleAttributeValueId { get; set; }

    }
}
