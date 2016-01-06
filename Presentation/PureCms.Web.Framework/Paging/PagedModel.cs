using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Web.Framework
{

    /// <summary>
    /// 分页模型
    /// </summary>
    public class PagedModel
    {
        private int _pageindex;//当前页索引
        private int _pagenumber;//当前页数
        private int _prepagenumber;//上一页数
        private int _nextpagenumber;//下一页数
        private int _pagesize;//每页数
        private int _totalcount;//总项数
        private int _totalpages;//总页数
        private bool _hasprepage;//是否有上一页
        private bool _hasnextpage;//是否有下一页
        private bool _isfirstpage;//是否是第一页
        private bool _islastpage;//是否是最后一页
        private string _linkUrl; //链接
        //public PagedModel() { }
        public PagedModel(int pageSize, int pageNumber, long totalCount, string linkurl = "")
        {
            if (pageSize > 0)
                _pagesize = pageSize;
            else
                _pagesize = 1;

            if (pageNumber > 0)
                _pagenumber = pageNumber;
            else
                _pagenumber = 1;

            if (totalCount > 0)
                _totalcount = (int)totalCount;
            else
                _totalcount = 0;

            _pageindex = _pagenumber - 1;

            _totalpages = _totalcount / _pagesize;
            if (_totalcount % _pagesize > 0)
                _totalpages++;

            _hasprepage = _pagenumber > 1;
            _hasnextpage = _pagenumber < _totalpages;

            _isfirstpage = _pagenumber == 1;
            _islastpage = _pagenumber == _totalpages;

            _prepagenumber = _pagenumber < 2 ? 1 : _pagenumber - 1;
            _nextpagenumber = _pagenumber < _totalpages ? _pagenumber + 1 : _totalpages;
            _linkUrl = linkurl;
        }

        //public string OutPageList()
        //{
        //    //计算页数
        //    if (TotalCount < 1 || PageSize < 1)
        //    {
        //        return "";
        //    }
        //    StringBuilder pageStr = new StringBuilder();
        //    string firstBtn = "<a href=\"" + string.Format(LinkUrl, PageNumber - 1) + "\" class=\"prev\">上一页</a>";
        //    string lastBtn = "<a href=\"" + string.Format(LinkUrl, PageNumber + 1) + "\" class=\"next\">后一页</a>";
        //    string firstStr = "<a href=\"" + string.Format(LinkUrl, "1") + "\">1</a>";
        //    string lastStr = "<a href=\"" + string.Format(LinkUrl, TotalPages) + "\">" + TotalPages.ToString() + "</a>";
        //    int centSize = 9; //中间页码数量

        //    if (PageNumber <= 1)
        //    {
        //        firstBtn = "<span class=\"current prev\">上一页</span>";
        //    }
        //    if (PageNumber >= TotalPages)
        //    {
        //        lastBtn = "<span class=\"current next\">下一页</span>";
        //    }
        //    if (PageNumber == 1)
        //    {
        //        firstStr = "<span class=\"current\">1</span>";
        //    }
        //    if (PageNumber == TotalPages)
        //    {
        //        lastStr = "<span class=\"current\">" + TotalPages.ToString() + "</span>";
        //    }
        //    int firstNum = PageNumber - (centSize / 2); //中间开始的页码
        //    if (PageNumber < centSize)
        //        firstNum = 2;
        //    int lastNum = PageNumber + centSize - ((centSize / 2) + 1); //中间结束的页码
        //    if (lastNum >= TotalPages)
        //        lastNum = TotalPages - 1;
        //    pageStr.Append(firstBtn + firstStr);
        //    if (PageNumber >= centSize)
        //    {
        //        pageStr.Append("<span>...</span>\n");
        //    }
        //    for (int i = firstNum; i <= lastNum; i++)
        //    {
        //        if (i == PageNumber)
        //        {
        //            pageStr.Append("<span class=\"current\">" + i + "</span>");
        //        }
        //        else
        //        {
        //            pageStr.Append("<a href=\"" + string.Format(LinkUrl, i) + "\">" + i + "</a>");
        //        }
        //    }
        //    if (TotalPages - PageNumber > centSize - ((centSize / 2)))
        //    {
        //        pageStr.Append("<span>...</span>");
        //    }
        //    pageStr.Append(TotalPages == 1 ? lastBtn : lastStr + lastBtn);
        //    return pageStr.ToString();
        //}

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return _pageindex; }
            set { _pageindex = value; }
        }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int PageNumber
        {
            get { return _pagenumber; }
            set { _pagenumber = value; }
        }
        /// <summary>
        /// 上一页数
        /// </summary>
        public int PrePageNumber
        {
            get { return _prepagenumber; }
            set { _prepagenumber = value; }
        }
        /// <summary>
        /// 下一页数
        /// </summary>
        public int NextPageNumber
        {
            get { return _nextpagenumber; }
            set { _nextpagenumber = value; }
        }
        /// <summary>
        /// 每页数
        /// </summary>
        public int PageSize
        {
            get { return _pagesize; }
            set { _pagesize = value; }
        }
        /// <summary>
        /// 总项数
        /// </summary>
        public int TotalCount
        {
            get { return _totalcount; }
            set { _totalcount = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get { return _totalpages; }
            set { _totalpages = value; }
        }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrePage
        {
            get { return _hasprepage; }
            set { _hasprepage = value; }
        }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get { return _hasnextpage; }
            set { _hasnextpage = value; }
        }
        /// <summary>
        /// 是否是第一页
        /// </summary>
        public bool IsFirstPage
        {
            get { return _isfirstpage; }
            set { _isfirstpage = value; }
        }
        /// <summary>
        /// 是否是最后一页
        /// </summary>
        public bool IsLastPage
        {
            get { return _islastpage; }
            set { _islastpage = value; }
        }

        /// <summary>
        /// 链接
        /// </summary>
        public string LinkUrl
        {
            get { return _linkUrl; }
            set { _linkUrl = value; }
        }
    }
}
