using PureCms.Core.Components.Platform;
using PureCms.Services.Component;
using PureCms.Services.Query;
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
        private GridService _gridService = new GridService();
        private FetchDataService _fetchService = new FetchDataService();

        public ActionResult Index()
        {
            return View();
        }
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
    }
}
