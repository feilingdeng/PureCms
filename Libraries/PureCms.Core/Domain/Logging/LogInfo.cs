using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using PureCms.Core.Domain.User;

namespace PureCms.Core.Domain.Logging
{
    [TableName("log")]
    [PrimaryKey("LogId",autoIncrement = true)]
    public class LogInfo :BaseEntity
    {
        public int LogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public int StatusCode { get; set; }

        public string ClientIP { get; set; }
        public string Url { get; set; }

        public string UrlReferrer { get; set; }

        #region 视图字段
        [ResultColumn()]
        [LinkEntity(typeof(UserInfo))]//, LinkFromFieldName = "userid", LinkToFieldName = "userid")]
        public string UserName { get; set; } 
        #endregion
    }
}
