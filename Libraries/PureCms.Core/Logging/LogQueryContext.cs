using PureCms.Core.Context;
using PureCms.Core.Domain.Logging;
using System;

namespace PureCms.Core.Logging
{
    public class LogQueryContext : QueryDescriptor<LogInfo, LogQueryContext>
    {

        /// <summary>
        /// 创建一个实例
        /// </summary>
        public LogQueryContext()// : base(this)
        {
        }
        //private LogInfo _mainEntityInfo;
        /// <summary>
        /// 实体属性
        /// </summary>
        //public LogInfo MainEntityInfo { 
        //    get{
        //        if(_mainEntityInfo == null)
        //        {
        //            _mainEntityInfo = new LogInfo();
        //        }
        //        return _mainEntityInfo;
        //    }
        //    set
        //    {
        //        _mainEntityInfo = value;
        //    }
        //}
        public string Title { get; set; }
        public string Description { get; set; }

        public int UserId { get; set; }
        public int StatusCode { get; set; }

        public string ClientIP { get; set; }
        public string Url { get; set; }

        public string UrlReferrer { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public override object GetConditionContainer()
        {
            return this;//.MainEntityInfo;
        }
    }
}
