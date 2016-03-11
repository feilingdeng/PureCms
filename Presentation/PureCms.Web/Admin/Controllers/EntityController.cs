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
        public ActionResult List(EntityGridModel model)
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
            model.Grid = _gridService.Build(queryView);
            var datas = _fetchService.Execute(model.Page, model.PageSize, queryView.FetchConfig);

            model.Items = datas.Items;
            model.TotalItems = datas.TotalItems;
            return View(model);
        }
    }
}
