using PureCms.Core.Context;
using PureCms.Core.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Security
{
    public class PrivilegeQueryContext : QueryDescriptor<PrivilegeInfo, PrivilegeQueryContext>
    {
        /// <summary>
        /// 
        /// </summary>
        public int PrivilegeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? ParentPrivilegeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        public int? Level { get; set; }
        public int? DisplayOrder { get; set; }

        public override object GetConditionContainer()
        {
            return this;
        }
    }
}
