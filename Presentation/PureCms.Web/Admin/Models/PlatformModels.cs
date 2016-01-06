using PureCms.Core.Domain.Logging;
using System;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Models
{
    //[ModelBinder(typeof(PagedModelBinder))]
    public class LogModel : BasePaged<LogInfo>
    {
        public int StatusCode { get; set; }

        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Url { get; set; }
        public string ClientIp { get; set; }

        public string Description { get; set; }
    }

    public class PagedModelBinder : IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type target = bindingContext.ModelType;
            object result = Activator.CreateInstance(target);
            foreach (var item in bindingContext.PropertyMetadata)
	        {
                if (controllerContext.RequestContext.HttpContext.Request[item.Key] != null)
                {
                    target.SetPropertyValue(result, item.Key, controllerContext.RequestContext.HttpContext.Request[item.Key]);
                }
	        }
            


            return result;
        }
    }
}