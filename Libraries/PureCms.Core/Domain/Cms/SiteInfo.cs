using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("sites")]
    [PrimaryKey("SiteId", autoIncrement = true)]
    public class SiteInfo : BaseEntity
    {
        public int SiteId { get; set; }
        public string Logo { get; set; }

        public string Name { get; set; }

        public string Theme { get; set; }

        public string Url { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
    }
}
