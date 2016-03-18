using PureCms.Core.Domain;
using PureCms.Core.Domain.Entity;
using PureCms.Core.Domain.Query;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Models
{
    public class ComponentModel : BasePaged<BaseEntity>
    {
        public string Type { get; set; }

        public WebGrid Grid { get; set; }

        public IHtmlString Table { get; set; }
    }
    public class EntityModel : BasePaged<EntityInfo>
    {
        public string Name { get; set; }
        public int ObjectTypeCode { get; set; }
        public bool IsLoged { get; set; }
        public bool IsCustomizable { get; set; }
        public bool IsAuthorization { get; set; }
        public string LocalizedName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
    }

    public class EditEntityModel
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }
        public bool IsLoged { get; set; }
        public bool IsAuthorization { get; set; }
        public string LocalizedName { get; set; }
    }

    public class AttributeModel : BasePaged<AttributeInfo>
    {
        public Guid? AttributeTypeId { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get; set; }
        public int Length { get; set; }
        public bool IsNullable { get; set; }
        public Guid? EntityId { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }
        public bool IsLoged { get; set; }
        public int? ReferencedEntityObjectTypeCode { get; set; }
        public Guid? OptionSetId { get; set; }
    }

    public class EditAttributeModel
    {
        public Guid? AttributeId { get; set; }
        public string AttributeType { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get; set; }
        public int Length { get; set; }
        public bool IsNullable { get; set; }
        public Guid EntityId { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRequired { get; set; }
        public bool IsLoged { get; set; }
        public Guid? ReferencedEntityId { get; set; }
        //public int? ReferencedEntityObjectTypeCode { get; set; }
        //public Guid? OptionSetId { get; set; }
        //public SelectList AttributeTypes { get; set; }

        //int setting
        public float? IntMinValue { get; set; }
        public float? IntMaxValue { get; set; }

        //nvarchar setting
        public string TextFormat { get; set; }
        public int? TextLength { get; set; }

        //float setting
        public int? FloatPrecision { get; set; }
        public float? FloatMinValue { get; set; }
        public float? FloatMaxValue { get; set; }

        //money setting
        public int? MoneyPrecision { get; set; }
        public float? MoneyMinValue { get; set; }
        public float? MoneyMaxValue { get; set; }

        //optionset setting
        public string OptionSetType { get; set; }
        public bool IsCommonOptionSet { set; get; }
        public Guid? CommonOptionSet { set; get; }
        public List<string> OptionSetName { set; get; }
        public List<int> OptionSetValue { set; get; }
        public List<bool> IsSelectedOption { set; get; }

        //bit setting
        public List<string> BitOptionName { get; set; }
        public List<bool> BitIsDefault { get; set; }

        //datetime setting
        public string DateTimeFormat { get; set; }

        //lookup setting
        public Guid? LookupEntity { get; set; }
    }

    public class QueryViewModel : BasePaged<QueryViewInfo>
    {
        public Guid QueryViewId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? OwnerId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSimpleFilter { get; set; }

        public Guid EntityId { get; set; }
    }

    public class EditQueryViewModel
    {
        public Guid QueryViewId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public string FetchConfig { get; set; }
        public string LayoutConfig { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? OwnerId { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSimpleFilter { get; set; }

        public Guid EntityId { get; set; }
        public EntityInfo EntityInfo { get; set; }
    }
    public class OptionSetModel : BasePaged<OptionSetInfo>
    {
        public Guid OptionSetId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        public List<OptionSetDetailInfo> Details { get; set; }
    }

    public class EditOptionSetModel
    {
        public Guid OptionSetId { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public List<OptionSetDetailInfo> Details { get; set; }
    }
    public class FormModel : BasePaged<SystemFormInfo>
    {
        public Guid EntityId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string FormConfig { get; set; }

        public bool CanBeDeleted { get; set; }
        public bool IsCustomizable { get; set; }
        public int StateCode { get; set; }
        public int FormType { get; set; }
    }
    public class EditFormModel
    {
        public Guid FormId { get; set; }
        public Guid EntityId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string FormConfig { get; set; }

        public bool CanBeDeleted { get; set; }
        public bool IsCustomizable { get; set; }
        public int StateCode { get; set; }
        public int FormType { get; set; }
    }
}