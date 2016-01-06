using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("ArticleCategories")]
    [PrimaryKey("ArticleCategoryId", autoIncrement = true)]
    public class ArticleCategoryInfo : BaseEntity
    {
        public int ArticleCategoryId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public int ParentArticleCategoryId { get; set; }

        public int Level { get; set; }
        public int DisplayOrder { get; set; }

        public bool HasChild { get; set; }
        public bool IsEnable { get; set; }
    }
}
