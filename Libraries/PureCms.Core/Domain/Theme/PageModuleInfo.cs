using PetaPoco;
using System;

namespace PureCms.Core.Domain.Theme
{
    [TableName("PageModules")]
    [PrimaryKey("PageModuleId")]
    public class PageModuleInfo : BaseEntity
    {
        public Guid PageModuleId { get; set; }

        public int ModuleId { get; set; }
        public int ThemeId { get; set; }
        public int PageId { get; set; }
        public int DisplayPosition { get; set; }
        public int DisplayOrder { get; set; }
        public Guid LayoutId { get; set; }

        public string Content { get; set; }
    }
}
