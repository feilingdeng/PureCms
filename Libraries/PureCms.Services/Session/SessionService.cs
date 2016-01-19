using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PureCms.Core.Session;
using System.Web;

namespace PureCms.Services.Session
{
    public class SessionService
    {
        private ISession _session;

        public SessionService(HttpContextBase httpContext)
        {
            _session = new AspNetSession(httpContext);
        }

        public void SetItem(string key, object value, int? Expiration)
        {
            _session.Set(key, value, Expiration);
        }

        public string GetValueString(string key)
        {
            var v = _session.Get(key);
            return v != null ? v.ToString() : string.Empty;
        }
        public int GetValueInt(string key)
        {
            var v = _session.Get(key);
            return v != null ? int.Parse(v.ToString()) : default(int);
        }
    }
}
