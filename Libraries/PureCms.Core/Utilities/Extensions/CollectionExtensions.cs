using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core
{
    public static class CollectionExtensions
    {
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> arg)
        {
            return arg != null && arg.Any();
        }
    }
}
