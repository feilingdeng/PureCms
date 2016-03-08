using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PureCms.Web.Framework
{
    public class GridView
    {
        public string _id;
        public GridView Id(string id) {
            this._id = id;
            return this;
        }
        public List<Column> _columns;
        public GridView Columns(List<Column> columns) {
            this._columns = columns;
            return this;
        }
        public string _url;
        public GridView Url(string url)
        {
            this._url = url;
            return this;
        }
        public bool _sortable;
        public GridView Sortable(bool sortable){
            this._sortable = sortable;
            return this;
        }
        public List<object> _items;
        public GridView Items(List<object> items)
        {
            this._items = items;
            return this;
        }
    }
    public class Column
    {
        public string Bind { get; set; }
        public bool Sortable { get; set; }
        public object HtmlAttributes { get; set; }//IDictionary<string, object> htmlAttributes
    }
}