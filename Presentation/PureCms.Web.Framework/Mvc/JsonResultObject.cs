using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Web.Framework
{
    public class JsonResultObject
    {
        public string StatusName { get; set; }
        public int StatusCode { get; set; }

        public string Content { get; set; }
    }
}
