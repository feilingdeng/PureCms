using PureCms.Web.Framework;
using System.Web.Mvc;
using PureCms.Core.Domain.Schema;
using PureCms.Web.Admin.Models;
using PureCms.Core.Context;
using System.EnterpriseServices;
using PureCms.Services.Schema;
using PureCms.Services.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureCms.Core.Domain.Query;
using PureCms.Core.Components.Platform;
using System.Web.Helpers;
using PureCms.Core.Domain.Entity;

namespace PureCms.Web.Admin.Controllers
{
    public class CustomizeController : BaseAdminController
    {
        static EntityService _entityService = new EntityService();
        static AttributeService _attributeService = new AttributeService();
        static AttributeTypeService _attributeTypeService = new AttributeTypeService();
        static QueryViewService _queryViewService = new QueryViewService();
        static OptionSetService _optionSetService = new OptionSetService();
        //static OptionSetDetailDetailService _optionSetDetailService = new OptionSetDetailDetailService();
        //
        // GET: /Customize/

        public ActionResult Index()
        {
            return View();//RedirectToAction("Entities");
        }

        #region 实体
        [Description("实体列表")]
        public ActionResult Entities(EntityModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<EntityInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<EntityInfo> container = new FilterContainer<EntityInfo>();
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<EntityInfo> result = _entityService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [Description("实体列表-JSON格式")]
        public ActionResult EntitiesJson(Guid? entityid)
        {
            FilterContainer<EntityInfo> filter = new FilterContainer<EntityInfo>();
            if(entityid.HasValue && !entityid.Equals(Guid.Empty))
            {
                filter.And(n => n.EntityId == entityid.Value);
            }
            List<EntityInfo> result = _entityService.Query(x => x
            .Select(n => new { n.EntityId, n.Name, n.LocalizedName })
                .Where(filter)
                .Sort(n => n.SortAscending(f => f.LocalizedName))
                );
            //相关联实体
            if (entityid.HasValue && !entityid.Equals(Guid.Empty))
            {
                var related = _entityService.QueryRelated(entityid.Value);
                result.AddRange(related);
            }

            return AjaxResult(true, result);
        }
        [Description("实体列表-JSON树格式")]
        public ActionResult EntitiesJsonTree(Guid? entityid)
        {
             var result = _entityService.GetJsonData(x => x
            .Select(n => new { n.EntityId, n.Name, n.LocalizedName })
                .Sort(n => n.SortAscending(f => f.Name)));
            return AjaxResult(true, result);
        }
        [Description("关系列表-JSON格式")]
        public ActionResult RelationsJson(Guid referencingEntityId)
        {
            List<RelationShipInfo> result = new RelationShipService().QueryByEntityId(referencingEntityId,null);

            return AjaxResult(true, result);
        }
        [HttpGet]
        [Description("新建实体")]
        public ActionResult CreateEntity()
        {
            EditEntityModel model = new EditEntityModel();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("新建实体-保存")]
        public ActionResult CreateEntity(EditEntityModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new EntityInfo();
                model.CopyTo(entity);
                entity.EntityId = Guid.NewGuid();
                _entityService.Create(entity);
                msg = "创建成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [HttpGet]
        [Description("实体编辑")]
        public ActionResult EditEntity(Guid id)
        {
            EditEntityModel model = new EditEntityModel();
            if (!id.Equals(Guid.Empty))
            {
                var entity = _entityService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                    return View(model);
                }
            }
            return NoRecordView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("实体信息保存")]
        public ActionResult EditEntity(EditEntityModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new EntityInfo();
                model.CopyTo(entity);

                _entityService.Update(entity);
                msg = "保存成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [Description("删除实体")]
        [HttpPost]
        public ActionResult DeleteEntity(Guid[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            flag = _entityService.DeleteById(recordid.ToList());
            if (flag)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return AjaxResult(flag, msg);
        }
        #endregion

        #region 字段
        [Description("字段列表")]
        public ActionResult Attributes(AttributeModel model)
        {
            if (model.EntityId.Equals(Guid.Empty))
            {
                return NoRecordView();
            }

            FilterContainer<AttributeInfo> container = new FilterContainer<AttributeInfo>();
            container.And(n => n.EntityId == model.EntityId);
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<AttributeInfo> result = _attributeService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [Description("字段列表-JSON格式")]
        public ActionResult AttributesJson(Guid entityid)
        {
            List<AttributeInfo> result = _attributeService.Query(x => x
            //.Select(n=>new {n.AttributeId,n.AttributeTypeId,n.AttributeTypeName,n.EntityId,n.EntityLocalizedName,n.EntityName,n.LocalizedName,n.Name })
                .Where(n => n.EntityId == entityid && n.Name != "versionnumber" && n.AttributeTypeId != Guid.Parse(AttributeTypeIds.PRIMARYKEY))
                .Sort(n => n.SortAscending(f => f.LocalizedName))
                );

            return AjaxResult(true, result);
        }
        [HttpGet]
        [Description("字段编辑")]
        public ActionResult CreateAttribute(Guid entityid)
        {
            if (entityid.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            EditAttributeModel model = new EditAttributeModel();
            //var attrTypes = _attributeTypeService.GetAll(q => q.Sort(s => s.SortAscending(ss => ss.Name)));
            //model.AttributeTypes = new SelectList(attrTypes, "AttributeTypeId", "Name");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("字段信息保存")]
        public ActionResult CreateAttribute(EditAttributeModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entityInfo = _entityService.FindById(model.EntityId);
                if (entityInfo == null)
                {
                    return NoRecordView();
                }
                var attrInfo = new AttributeInfo();
                //model.CopyTo(entity);
                attrInfo.EntityId = entityInfo.EntityId;
                attrInfo.EntityName = entityInfo.Name;
                attrInfo.ReferencedEntityObjectTypeCode = entityInfo.ObjectTypeCode;
                attrInfo.Name = model.Name;
                attrInfo.LocalizedName = model.LocalizedName;
                attrInfo.AttributeId = Guid.NewGuid();
                attrInfo.IsNullable = model.IsNullable;
                attrInfo.IsRequired = model.IsRequired;
                attrInfo.IsLoged = model.IsLoged;
                switch (model.AttributeType)
                {
                    case "nvarchar":
                        attrInfo.MaxLength = model.TextLength.Value;
                        attrInfo.DataFormat = model.TextFormat;
                        break;
                    case "ntext":
                        break;
                    case "int":
                        attrInfo.MinValue = model.IntMinValue.Value;
                        attrInfo.MaxValue = model.IntMaxValue.Value;
                        break;
                    case "float":
                        attrInfo.Precision = model.FloatPrecision.Value;
                        attrInfo.MinValue = model.FloatMinValue.Value;
                        attrInfo.MaxValue = model.FloatMaxValue.Value;
                        break;
                    case "money":
                        attrInfo.Precision = model.MoneyPrecision.Value;
                        attrInfo.MinValue = model.MoneyMinValue.Value;
                        attrInfo.MaxValue = model.MoneyMaxValue.Value;
                        break;
                    case "optionset":
                        attrInfo.DisplayStyle = model.OptionSetType;
                        if (model.IsCommonOptionSet)
                        {
                            attrInfo.OptionSetId = model.CommonOptionSet.Value;
                        }
                        else
                        {
                            //新建选项集
                            OptionSetInfo os = new OptionSetInfo();
                            os.OptionSetId = Guid.NewGuid();
                            os.Name = model.Name;
                            os.IsPublic = false;
                            List<OptionSetDetailInfo> details = new List<OptionSetDetailInfo>();
                            int i = 0;
                            foreach (var item in model.OptionSetName)
                            {
                                OptionSetDetailInfo osd = new OptionSetDetailInfo();
                                osd.OptionSetDetailId = Guid.NewGuid();
                                osd.OptionSetId = os.OptionSetId;
                                osd.Name = item;
                                osd.Value = model.OptionSetValue[i];
                                osd.IsSelected = model.IsSelectedOption[i];
                                details.Add(osd);
                                i++;
                            }
                            if (_optionSetService.Create(os, details))
                            {
                                attrInfo.OptionSetId = os.OptionSetId;
                            }
                            else
                            {

                            }
                        }
                        break;
                    case "bit":
                        //新建选项集
                        OptionSetInfo bitos = new OptionSetInfo();
                        bitos.OptionSetId = Guid.NewGuid();
                        bitos.Name = model.Name;
                        bitos.IsPublic = false;
                        List<OptionSetDetailInfo> bitdetails = new List<OptionSetDetailInfo>();
                        int j = 0;
                        foreach (var item in model.BitOptionName)
                        {
                            OptionSetDetailInfo osd = new OptionSetDetailInfo();
                            osd.OptionSetDetailId = Guid.NewGuid();
                            osd.OptionSetId = bitos.OptionSetId;
                            osd.Name = item;
                            osd.Value = model.BitIsDefault[j] ? 1 : 0;
                            osd.IsSelected = model.BitIsDefault[j];
                            bitdetails.Add(osd);
                            j++;
                        }
                        if (_optionSetService.Create(bitos, bitdetails))
                        {
                            attrInfo.OptionSetId = bitos.OptionSetId;
                        }
                        else
                        {

                        }
                        break;
                    case "datetime":
                        attrInfo.DataFormat = model.DateTimeFormat;
                        break;
                    case "lookup":
                        attrInfo.ReferencedEntityId = model.LookupEntity.Value;
                        break;
                }
                var attributeType = new AttributeTypeService().FindByName(model.AttributeType);
                attrInfo.AttributeTypeId = attributeType.AttributeTypeId;
                attrInfo.AttributeTypeName = model.AttributeType;
                _attributeService.Create(attrInfo);
                msg = "创建成功";
                return AjaxResult(true, msg);
            }
            else
            {
                msg = GetModelErrors(ModelState);
                return AjaxResult(false, "保存失败: " + msg);
            }
        }
        [HttpGet]
        [Description("字段编辑")]
        public ActionResult EditAttribute(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            EditAttributeModel model = new EditAttributeModel();
            var entity = _attributeService.FindById(id);
            if (entity != null)
            {
                entity.CopyTo(model);
                return View(model);
            }
            else {
                return NoRecordView();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("字段信息保存")]
        public ActionResult EditAttribute(EditAttributeModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new AttributeInfo();
                model.CopyTo(entity);

                _attributeService.Update(entity);
                msg = "保存成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [Description("删除字段")]
        [HttpPost]
        public ActionResult DeleteAttribute(Guid[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            flag = _attributeService.DeleteById(recordid.ToList());
            if (flag)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return AjaxResult(flag, msg);
        }
        #endregion

        #region 视图
        [Description("字段类型对应的操作符")]
        public ActionResult OperatorJson(string attributeType)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            switch (attributeType)
            {
                case "nvarchar":
                    result = AttributeTypes.StringOperators;
                    break;
                case "int":
                    result = AttributeTypes.NumberOperators;
                    break;
                case "float":
                    result = AttributeTypes.NumberOperators;
                    break;
                case "decimal":
                    result = AttributeTypes.NumberOperators;
                    break;
                case "money":
                    result = AttributeTypes.NumberOperators;
                    break;
                case "picklist":
                    result = AttributeTypes.PickListOperators;
                    break;
                case "bit":
                    result = AttributeTypes.PickListOperators;
                    break;
                case "state":
                    result = AttributeTypes.PickListOperators;
                    break;
                case "status":
                    result = AttributeTypes.PickListOperators;
                    break;
                case "lookup":
                    result = AttributeTypes.LookUpOperators;
                    break;
                case "uniqueidentifier":
                    result = AttributeTypes.LookUpOperators;
                    break;
                case "primarykey":
                    result = AttributeTypes.LookUpOperators;
                    break;
                case "owner":
                    result = AttributeTypes.LookUpOperators;
                    break;
                case "datetime":
                    result = AttributeTypes.DateTimeOperators;
                    break;
            }

            return AjaxResult(true, result);
        }
        [Description("视图列表")]
        public ActionResult QueryViews(QueryViewModel model)
        {
            var entity = _entityService.FindById(model.EntityId);
            if (entity == null)
            {
                return NoRecordView();
            }
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<QueryViewInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<QueryViewInfo> container = new FilterContainer<QueryViewInfo>();
            container.And(n => n.EntityId == model.EntityId);
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<QueryViewInfo> result = _queryViewService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [HttpGet]
        [Description("新建视图")]
        public ActionResult CreateQueryView(Guid entityid)
        {
            if (entityid.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            var entity = _entityService.FindById(entityid);
            if (entity == null)
            {
                return NoRecordView();
            }
            EditQueryViewModel model = new EditQueryViewModel();
            model.EntityId = entityid;
            model.EntityInfo = entity;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("新建视图-保存")]
        public ActionResult CreateQueryView(EditQueryViewModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new QueryViewInfo();
                model.CopyTo(entity);
                entity.QueryViewId = Guid.NewGuid();
                _queryViewService.Create(entity);
                msg = "创建成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [HttpGet]
        [Description("视图编辑")]
        public ActionResult EditQueryView(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            EditQueryViewModel model = new EditQueryViewModel();
            if (!id.Equals(Guid.Empty))
            {
                var entity = _queryViewService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                    return View(model);
                }
            }
            return NoRecordView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("视图信息保存")]
        public ActionResult EditQueryView(EditQueryViewModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new QueryViewInfo();
                model.CopyTo(entity);

                _queryViewService.Update(entity);
                msg = "保存成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [Description("删除视图")]
        [HttpPost]
        public ActionResult DeleteQueryView(Guid[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            flag = _queryViewService.DeleteById(recordid.ToList());
            if (flag)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return AjaxResult(flag, msg);
        }
        #endregion

        #region 选项集
        [Description("选项集列表")]
        public ActionResult OptionSets(OptionSetModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<OptionSetInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<OptionSetInfo> filter = new FilterContainer<OptionSetInfo>();
            filter.And(n=>n.IsPublic == true);
            if (model.Name.IsNotEmpty())
            {
                filter.And(n => n.Name.Like(model.Name));
            }
            PagedList<OptionSetInfo> result = _optionSetService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(filter)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [Description("选项集列表-JSON格式")]
        public ActionResult OptionSetsJson(Guid? id, bool? ispublic)
        {
            FilterContainer<OptionSetInfo> filter = new FilterContainer<OptionSetInfo>();
            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                filter.And(n => n.OptionSetId == id.Value);
            }
            if (ispublic.HasValue)
            {
                filter.And(n => n.IsPublic == ispublic.Value);
            }
            List<OptionSetInfo> result = _optionSetService.Query(x => x
                .Where(filter)
                .Sort(n => n.SortAscending(f => f.Name))
                );

            return AjaxResult(true, result);
        }
        [HttpGet]
        [Description("新建选项集")]
        public ActionResult CreateOptionSet()
        {
            EditOptionSetModel model = new EditOptionSetModel();

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("新建选项集-保存")]
        public ActionResult CreateOptionSet(EditOptionSetModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new OptionSetInfo();
                model.CopyTo(entity);
                entity.OptionSetId = Guid.NewGuid();
                _optionSetService.Create(entity);
                msg = "创建成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [HttpGet]
        [Description("选项集编辑")]
        public ActionResult EditOptionSet(Guid id)
        {
            EditOptionSetModel model = new EditOptionSetModel();
            if (!id.Equals(Guid.Empty))
            {
                var entity = _optionSetService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                    return View(model);
                }
            }
            return NoRecordView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("选项集信息保存")]
        public ActionResult EditOptionSet(EditOptionSetModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new OptionSetInfo();
                model.CopyTo(entity);

                _optionSetService.Update(entity);
                msg = "保存成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [Description("删除选项集")]
        [HttpPost]
        public ActionResult DeleteOptionSet(Guid[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            flag = _optionSetService.DeleteById(recordid.ToList());
            if (flag)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return AjaxResult(flag, msg);
        }
        #endregion

        #region 表单
        [Description("表单列表")]
        public ActionResult Forms(OptionSetModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<OptionSetInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<OptionSetInfo> filter = new FilterContainer<OptionSetInfo>();
            filter.And(n => n.IsPublic == true);
            if (model.Name.IsNotEmpty())
            {
                filter.And(n => n.Name.Like(model.Name));
            }
            PagedList<OptionSetInfo> result = _optionSetService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(filter)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        [Description("表单列表-JSON格式")]
        public ActionResult FormsJson(Guid? id, bool? ispublic)
        {
            FilterContainer<OptionSetInfo> filter = new FilterContainer<OptionSetInfo>();
            if (id.HasValue && !id.Equals(Guid.Empty))
            {
                filter.And(n => n.OptionSetId == id.Value);
            }
            if (ispublic.HasValue)
            {
                filter.And(n => n.IsPublic == ispublic.Value);
            }
            List<OptionSetInfo> result = _optionSetService.Query(x => x
                .Where(filter)
                .Sort(n => n.SortAscending(f => f.Name))
                );

            return AjaxResult(true, result);
        }
        [HttpGet]
        [Description("新建表单")]
        public ActionResult CreateForm(Guid entityid)
        {
            EditFormModel model = new EditFormModel();
            model.EntityId = entityid;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("新建表单-保存")]
        public ActionResult CreateForm(EditFormModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new SystemFormInfo();
                model.CopyTo(entity);
                entity.SystemFormId = Guid.NewGuid();
                //_optionSetService.Create(entity);
                msg = "创建成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [HttpGet]
        [Description("表单编辑")]
        public ActionResult EditForm(Guid id)
        {
            EditFormModel model = new EditFormModel();
            if (!id.Equals(Guid.Empty))
            {
                var entity = _optionSetService.FindById(id);
                if (entity != null)
                {
                    entity.CopyTo(model);
                    return View(model);
                }
            }
            return NoRecordView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Description("表单信息保存")]
        public ActionResult EditForm(EditFormModel model)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new EditFormModel();
                model.CopyTo(entity);

                //_optionSetService.Update(entity);
                msg = "保存成功";
                return AjaxResult(true, msg);
            }
            msg = GetModelErrors(ModelState);
            return AjaxResult(false, "保存失败: " + msg);
        }
        [Description("删除表单")]
        [HttpPost]
        public ActionResult DeleteForm(Guid[] recordid)
        {
            string msg = string.Empty;
            bool flag = false;
            flag = _optionSetService.DeleteById(recordid.ToList());
            if (flag)
            {
                msg = "删除成功";
            }
            else
            {
                msg = "删除失败";
            }
            return AjaxResult(flag, msg);
        }
        #endregion


        #region grid
        public ActionResult QueryViewGrid(QueryViewModel model)
        {
            var entity = _entityService.FindById(model.EntityId);
            if (entity == null)
            {
                return NoRecordView();
            }
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<QueryViewInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<QueryViewInfo> container = new FilterContainer<QueryViewInfo>();
            container.And(n => n.EntityId == model.EntityId);
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<QueryViewInfo> result = _queryViewService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        public ActionResult OptionSetGrid(OptionSetModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<OptionSetInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<OptionSetInfo> filter = new FilterContainer<OptionSetInfo>();
            filter.And(n => n.IsPublic == true);
            if (model.Name.IsNotEmpty())
            {
                filter.And(n => n.Name.Like(model.Name));
            }
            PagedList<OptionSetInfo> result = _optionSetService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(filter)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        public ActionResult EntityGrid(EntityModel model)
        {
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<EntityInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<EntityInfo> container = new FilterContainer<EntityInfo>();
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<EntityInfo> result = _entityService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        public ActionResult AttributeGrid(AttributeModel model)
        {
            if (model.EntityId.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            if (model.SortBy.IsEmpty())
            {
                model.SortBy = Utilities.ExpressionHelper.GetPropertyName<AttributeInfo>(n => n.CreatedOn);
                model.SortDirection = (int)Core.Context.SortDirection.Desc;
            }

            FilterContainer<AttributeInfo> container = new FilterContainer<AttributeInfo>();
            container.And(n => n.EntityId == model.EntityId);
            if (model.Name.IsNotEmpty())
            {
                container.And(n => n.Name == model.Name);
            }
            PagedList<AttributeInfo> result = _attributeService.QueryPaged(x => x
                .Page(model.Page, model.PageSize)
                .Where(container)
                .Sort(n => n.OnFile(model.SortBy).ByDirection(model.SortDirection))
                );

            model.Items = result.Items;
            model.TotalItems = result.TotalItems;
            return View(model);
        }
        #endregion
    }
}
