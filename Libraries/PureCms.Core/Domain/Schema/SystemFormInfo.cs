using System;
using PetaPoco;

namespace PureCms.Core.Domain.Schema
{
    [TableName("systemform")]
    [PrimaryKey("systemformid", autoIncrement = false)]
    public partial class SystemFormInfo : BaseEntity
    {
        public Guid SystemFormId { get; set; }
        public string Name { get; set; }

        public DateTime? PublishedOn { get; set; }

        public string FormConfig { get; set; }

        public bool CanBeDeleted { get; set; }
        public bool IsCustomizable { get; set; }
        public bool IsDefault { get; set; }

        public Guid OrganizationId { get; set; }

        public int StateCode { get; set; }
        public int FormType { get; set; }
        [ResultColumn]
        public int ObjectTypeCode { get; set; }
        [ResultColumn]
        public string EntityName { get; set; }
        public Guid EntityId { get; set; }
        public string Description { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
