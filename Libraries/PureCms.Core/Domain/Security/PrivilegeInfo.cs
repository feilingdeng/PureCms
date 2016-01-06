using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace PureCms.Core.Domain.Security
{
    [TableName("privileges")]
    [PrimaryKey("PrivilegeId", autoIncrement = true)]
    public class PrivilegeInfo : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int PrivilegeId { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ParentPrivilegeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OpenTarget { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsShowAsMenu { get; set; }

        public string Description { get; set; }


        /// <summary>
        /// 小图标
        /// </summary>
        public string SmallIcon { get; set; }

        /// <summary>
        /// 大图标
        /// </summary>
        public string BigIcon { get; set; }

        public int Level { get; set; }
    }
}
