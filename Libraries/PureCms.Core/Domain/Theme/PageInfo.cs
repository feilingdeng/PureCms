using PetaPoco;

namespace PureCms.Core.Domain.Theme
{
    [TableName("Pages")]
    [PrimaryKey("PageId", autoIncrement = true)]
    public class PageInfo : BaseEntity
    {
        public int PageId { get; set; }
        public int ThemeId { get; set; }

        public string Name { get; set; }
        public string PageUrl { get; set; }
        public int PageType { get; set; }
        
        public bool IsIndex { get; set; }

        public int LayoutType { get; set; }

        public bool IsEnabled { get; set; }
    }
}
