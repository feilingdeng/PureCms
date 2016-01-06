using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("themes")]
    [PrimaryKey("ThemeId", autoIncrement = true)]
    public class ThemeInfo : BaseEntity
    {
        public int ThemeId { get; set; }

        public string PathName { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
        public string Author { get; set; }
        public string Picture { get; set; }

        public string Version { get; set; }

        public bool IsEnabled { get; set; }
    }
}
