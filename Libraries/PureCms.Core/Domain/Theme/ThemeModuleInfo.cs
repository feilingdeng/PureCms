using PetaPoco;

namespace PureCms.Core.Domain.Theme
{
    [TableName("ThemeModules")]
    [PrimaryKey("ModuleId", autoIncrement = true)]
    public class ThemeModuleInfo : BaseEntity
    {
        public int ModuleId { get; set; }

        public int ThemeId { get; set; }

        public string Name { get; set; }

        public int PageType { get; set; }

        public int EnabledPosition { get; set; }

        public int ModuleType { get; set; }

        public bool IsEnabled { get; set; }
    }
}
