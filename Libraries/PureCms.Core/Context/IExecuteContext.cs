
using PureCms.Core.Domain;
using System.Collections.Generic;
namespace PureCms.Core.Context
{
    public interface IExecuteContext<T> where T : new()//BaseEntity
    {
        int TopCount { get; set; }
        /// <summary>
        /// 分页信息
        /// </summary>
        PageDescriptor PagingInfo { get; set; }
        /// <summary>
        /// 排序信息
        /// </summary>
        //List<SortDescriptor<T>> SortingInfo { get; set; }
        /// <summary>
        /// 其它数据
        /// </summary>
        List<Dictionary<string,object>> Extras { get; set; }
        /// <summary>
        /// 执行容器对象
        /// </summary>
        object ExecuteContainer { get; set; }
    }
}
