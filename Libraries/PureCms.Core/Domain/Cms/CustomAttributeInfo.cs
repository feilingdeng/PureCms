using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("CustomAttributes")]
    [PrimaryKey("CustomAttributeId", autoIncrement = true)]
    public class CustomAttributeInfo : BaseEntity
    {
        public int CustomAttributeId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
        public string DefaultValue { get; set; }

        public int ControlType { get; set; }
        public int DataType { get; set; }
        public int DataLength { get; set; }
        public bool IsRequired { get; set; }
        public string ValidText { get; set; }
        public bool IsEnabled { get; set; }
    }
}
