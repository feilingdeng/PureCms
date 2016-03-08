using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms
{
    public static class CollectionExtensions
    {
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> arg)
        {
            return arg != null && arg.Any();
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return (source == null || source.Count == 0);
        }
        /// <summary>
        /// 组合转换为字符串
        /// </summary>
        /// <param name="items"></param>
        /// <param name="separator">分隔符</param>
        /// <param name="wrapItem">项目包裹符</param>
        /// <returns></returns>
        public static string CollectionToString(this ICollection<object> items, string separator, string wrapItem = "")
        {
            string result = string.Empty;
            if (wrapItem.IsNotEmpty())
            {
                var newList = new List<object>();
                foreach (var item in items)
                {
                    newList.Add(wrapItem + item + wrapItem);
                }
                result = string.Join(separator, newList);
            }
            else
            {
                result = string.Join(separator, items);
            }
            return result;
        }
    }
}
