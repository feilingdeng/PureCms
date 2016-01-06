using System.Collections.Generic;

namespace PureCms.Core.Context
{
    public class PagedList<T>
    {
        public PagedList() { }
        public long CurrentPage { get; set; }
        public List<T> Items { get; set; }
        public long ItemsPerPage { get; set; }
        public long TotalItems { get; set; }
        public long TotalPages { get; set; }
    }
}
