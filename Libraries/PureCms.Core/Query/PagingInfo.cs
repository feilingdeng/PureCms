using System.Runtime.Serialization;

namespace PureCms.Core.Query
{
    public sealed class PagingInfo
    {
        private int _pageNumber;
        private int _count;
        private string _pagingCookie;
        private bool _returnTotalRecordCount;
        [DataMember]
        public int PageNumber
        {
            get
            {
                return this._pageNumber;
            }
            set
            {
                this._pageNumber = value;
            }
        }
        [DataMember]
        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                this._count = value;
            }
        }
        [DataMember]
        public bool ReturnTotalRecordCount
        {
            get
            {
                return this._returnTotalRecordCount;
            }
            set
            {
                this._returnTotalRecordCount = value;
            }
        }
        [DataMember]
        public string PagingCookie
        {
            get
            {
                return this._pagingCookie;
            }
            set
            {
                this._pagingCookie = value;
            }
        }
    }
}
