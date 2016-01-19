using System.Web;

namespace PureCms.Core.Session
{
    public class AspNetSession : ISession
    {
        private HttpContextBase _httpContext;
        public AspNetSession(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
        }
        public void Clear()
        {
            _httpContext.Session.Clear();
        }

        public T Get<T>(string key) where T : class, new()
        {
            T t = default(T);
            var value = _httpContext.Session[key];
            if (null != value)
            {
                t = value as T;
            }
            return t;
        }

        public object Get(string key)
        {
            return _httpContext.Session[key];
        }

        public void Remove(string key)
        {
            _httpContext.Session.Remove(key);
        }

        public void Set(string key, object value, int? Expiration)
        {
            _httpContext.Session[key] = value;
        }
    }
}
