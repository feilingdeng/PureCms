using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PureCms.Web.Models
{
    public class PageModel
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
    public class EditPageModel
    {
        public int? PageId { get; set; }
        public int ThemeId { get; set; }

        public string Name { get; set; }
        public string PageUrl { get; set; }
        public int PageType { get; set; }

        public bool IsIndex { get; set; }

        public int LayoutType { get; set; }

        public bool IsEnabled { get; set; }
    }

    public class LayoutModel
    {
        public Guid LayoutId { get; set; }

        public int PageId { get; set; }
        public int DisplayOrder { get; set; }
        public int LayoutType { get; set; }
    }
    public class EditLayoutModel
    {
        public Guid? LayoutId { get; set; }

        public int PageId { get; set; }
        public int DisplayOrder { get; set; }
        public int LayoutType { get; set; }
    }

    public class ModuleModel
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
    public class EditModuleModel
    {
        public Guid? PageModuleId { get; set; }

        public int ModuleId { get; set; }
        public int ThemeId { get; set; }
        public int PageId { get; set; }
        public int DisplayPosition { get; set; }
        public int DisplayOrder { get; set; }
        public Guid LayoutId { get; set; }

        public string Content { get; set; }
    }

}