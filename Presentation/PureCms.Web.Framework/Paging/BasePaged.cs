using PureCms.Core.Context;
using PureCms.Core.Domain;
using PureCms.Core.Domain.Logging;
using PureCms.Web.Framework;
using System.Collections.Generic;

namespace PureCms.Web.Admin.Models
{
    public class BasePaged<T> where T : BaseEntity
    {
        public long TotalItems { get; set; }
        //public PagedList<T> PagedResult { get; set; }

        public List<T> Items { get; set; }

        private List<SortDescriptor<T>> _sortingInfo;
        /// <summary>
        /// 排序信息
        /// </summary>
        public List<SortDescriptor<T>> SortingInfo
        {
            get
            {
                if(_sortingInfo == null)
                {
                    _sortingInfo = new List<SortDescriptor<T>>();
                }
                return _sortingInfo;
            }
            set
            {
                _sortingInfo = value;
            }
        }

        private PagedModel _pagedModel;
        /// <summary>
        /// 分页对象
        /// </summary>
        public PagedModel PagedModel {
            get
            {
                if(_pagedModel == null)
                {
                    return new PagedModel(this.PageSize, this.Page, this.TotalItems);
                }
                return _pagedModel;
            }
            set
            {
                this._pagedModel = value;
            }
        }
        private int _page = 1;
        public int Page
        {
            get{
                return _page > 0 ? _page : 1;
            }
            set{
                _page = value;
            }
        }
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize > 0 ? _pageSize : 10;
            }
            set
            {
                _pageSize = value;
            }
        }

        public string SortBy
        {
            get;
            set;
        }
        public int SortDirection
        {
            get;
            set;
        }
    }

    
}