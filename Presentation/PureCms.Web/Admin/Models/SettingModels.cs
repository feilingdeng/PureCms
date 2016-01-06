using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PureCms.Web.Admin.Models
{
    public class PlatformSettingModel
    {
        public string SiteName { get; set; }
        public string Url { get; set; }

        public int Status { get; set; }

        public string ClosedReason { get; set; }
    }
}