using PetaPoco;

namespace PureCms.Core.Domain.Schema
{
    [TableName("information_schema.columns")]
    public class ColumnInfo : BaseEntity
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public int DataLength { get; set; }
    }
}
