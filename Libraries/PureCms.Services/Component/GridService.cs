using PureCms.Core.Components.Grid;
using PureCms.Core.Domain.Query;
using PureCms.Services.Query;
using PureCms.Services.Schema;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PureCms.Services.Component
{
    public class GridService
    {
        private GridBuilder _gridBuilder;
        private FetchDataService _fetchService = new FetchDataService();
        private AttributeService _attributeService = new AttributeService();

        public GridService()
        {

        }

        public GridInfo Build(int page, int pageSize, Guid queryid)
        {
            var queryView = new QueryViewService().FindById(queryid);
            //var sql = _fetchService.ToSqlString(_fetchService.ToQueryExpression(queryView.FetchConfig));
            _gridBuilder = new GridBuilder(queryView);
            //_gridBuilder.Grid.DataSource = _fetchService.Execute(page, pageSize, queryView.FetchConfig);
            return _gridBuilder.Grid;
        }
        public GridInfo Build(QueryViewInfo view)
        {
            _gridBuilder = new GridBuilder(view);
            //grid中列名赋值为实体字段的显示名称
            var attributes = _attributeService.Query(q=>q.Select(n => new { n.Name,n.LocalizedName,n.EntityId,n.EntityName,n.EntityLocalizedName,n.AttributeTypeId,n.AttributeTypeName })
            .Where(w=>w.EntityId == view.EntityId)
            );
            GridInfo grid = _gridBuilder.Grid;
            int i = 0;
            foreach (var cell in grid.Rows[0].Cells)
            {
                var attr = attributes.Find(x=>x.Name == cell.Name);
                if(attr != null)
                {
                    grid.Rows[0].Cells[i].Label = attr.LocalizedName;
                }
                i++;
            }

            return grid;
        }
    }
}
