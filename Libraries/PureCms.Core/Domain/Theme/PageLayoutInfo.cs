using PetaPoco;
using System;

namespace PureCms.Core.Domain.Theme
{
    [TableName("PageLayouts")]
    [PrimaryKey("LayoutId", autoIncrement = false)]
    public class PageLayoutInfo : BaseEntity
    {
        public Guid LayoutId { get; set; }

        public int PageId { get; set; }
        public int DisplayOrder { get; set; }
        public int LayoutType { get; set; }
    }
}
