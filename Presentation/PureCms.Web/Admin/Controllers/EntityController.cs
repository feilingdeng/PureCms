using PureCms.Core.Components.Form;
using PureCms.Core.Components.Platform;
using PureCms.Core.Domain.Schema;
using PureCms.Services.Component;
using PureCms.Services.Schema;
using PureCms.Web.Admin.Models;
using PureCms.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Controllers
{
    public class EntityController : BaseAdminController
    {
        private static EntityService _entityService = new EntityService();
        private static GridService _gridService = new GridService();
        private static FetchDataService _fetchService = new FetchDataService();
        private static SystemFormService _formService = new SystemFormService();

        public ActionResult Index()
        {
            return View();
        }
        #region 列表
        public ActionResult List(Guid queryid)
        {
            if (queryid.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            EntityGridModel model = new EntityGridModel();
            model.QueryId = queryid;
            return View(model);
        }
        public ActionResult GridView(EntityGridModel model)
        {
            if (model.QueryId.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            var queryView = new QueryViewService().FindById(model.QueryId);
            if (queryView == null)
            {
                return NoRecordView();
            }
            model.QueryView = queryView;
            QueryColumnSortInfo sort = new QueryColumnSortInfo(model.SortBy, model.SortDirection == 0);

            var datas = _fetchService.Execute(model.Page, model.PageSize, sort, queryView);
            model.Grid = _gridService.Build(queryView, _fetchService.EntityList, _fetchService.AttributeList);
            model.EntityList = _fetchService.EntityList;
            model.AttributeList = _fetchService.AttributeList;
            model.Items = datas.Items;
            model.TotalItems = datas.TotalItems;
            return View(model);
        }
        #endregion

        #region 新建记录
        public ActionResult Create(Guid entityid, Guid? formid)
        {
            if (entityid.Equals(Guid.Empty))
            {
                return NoRecordView();
            }
            CreateRecordModel model = new CreateRecordModel();
            var entityInfo = _entityService.FindById(entityid);
            model.EntityInfo = entityInfo;
            model.EntityId = entityid;
            var formEntity = new SystemFormInfo();
            if (formid.HasValue)
            {
                formEntity = _formService.FindById(formid.Value);
            }
            else
            {
                //获取实体默认表单

            }
            model.FormInfo = formEntity;
            FormBuilder _formBuilder = new FormBuilder(formEntity);
            var _form = _formBuilder.Form;
            model.Form = _form;

            return View(model);
        }
        //public ActionResult GetFormJson(Guid entityid, Guid? formid)
        //{
        //    if (entityid.Equals(Guid.Empty))
        //    {
        //        return NoRecordView();
        //    }
        //    CreateRecordModel model = new CreateRecordModel();
        //    var entityInfo = _entityService.FindById(entityid);
        //    model.EntityInfo = entityInfo;
        //    model.EntityId = entityid;
        //    var formEntity = new SystemFormInfo();
        //    if (formid.HasValue)
        //    {
        //        formEntity = _formService.FindById(formid.Value);
        //    }
        //    else
        //    {
        //        //获取实体默认表单

        //    }
        //    FormBuilder _formBuilder = new FormBuilder(formEntity);
        //    var _form = _formBuilder.Form;
        //    model.Form = _form;
        //    return AjaxResult(true, model);
        //}
        #endregion
    }
}
