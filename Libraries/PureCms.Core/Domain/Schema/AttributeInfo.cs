using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using PetaPoco;

namespace PureCms.Core.Domain.Schema
{
    [TableName("Attribute")]
    [PrimaryKey("AttributeId", autoIncrement = false)]
    public class AttributeInfo : BaseEntity
    {
        public Guid AttributeId { get; set; }
        public Guid AttributeTypeId { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get; set; }
        public int MaxLength { get; set; }
        public float MinValue { get; set; }

        public float MaxValue { get; set; }
        public string DataFormat { get; set; }
        public string DisplayStyle { get; set; }
        public int Precision { get; set; }
        public bool IsNullable { get; set; }
        public Guid EntityId { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }
        public bool IsLoged { get; set; }
        public int? ReferencedEntityObjectTypeCode { get; set; }
        public Guid? OptionSetId { get; set; }
        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "EntityId", LinkToFieldName = "EntityId", TargetFieldName = "ObjectTypeCode")]
        public int ObjectTypeCode { get; set; }
        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "EntityId", LinkToFieldName = "EntityId", TargetFieldName = "LocalizedName")]
        public string EntityLocalizedName { get; set; }
        [ResultColumn]
        [LinkEntity(typeof(EntityInfo), LinkFromFieldName = "EntityId", LinkToFieldName = "EntityId", TargetFieldName = "Name")]
        public string EntityName { get; set; }
        [ResultColumn]
        [LinkEntity(typeof(AttributeTypeInfo), LinkFromFieldName = "AttributeTypeId", LinkToFieldName = "AttributeTypeId", TargetFieldName = "Name")]
        public string AttributeTypeName { get; set; }

        private string _PickListItem;
        [ResultColumn]
        public string PickListItem
        {
            get
            {
                return _PickListItem;
            }
            set
            {
                this._PickListItem = value;
                //if (!string.IsNullOrWhiteSpace(value))
                //{
                //    XElement root = XElement.Parse("<root>" + value + "</root>");
                //    var nodes = from el in root.Descendants("StringMap") select el;
                //    List<OptionSetDetailInfo> list = new List<OptionSetDetailInfo>(nodes.Count());
                //    foreach (XElement item in nodes)
                //    {
                //        OptionSetDetailInfo sm = new OptionSetDetailInfo();
                //        sm.AttributeId = Guid.Parse(item.Attribute("AttributeId").Value);
                //        sm.DisplayOrder = int.Parse(item.Attribute("DisplayOrder").Value);
                //        sm.Name = item.Attribute("Name").Value;
                //        sm.ObjectTypeCode = int.Parse(item.Attribute("ObjectTypeCode").Value);
                //        sm.Value = int.Parse(item.Attribute("Value").Value);

                //        list.Add(sm);
                //    }
                //    this.PickLists = list;
                //}
            }
        }
        [ResultColumn]
        public List<OptionSetDetailInfo> PickLists { get; set; }
    }
}
