namespace PureCms.Core.Session
{
    public interface ISession
    {
        void Set(string key, object value, int? Expiration);

        T Get<T>(string key) where T : class, new();

        object Get(string key);

        void Remove(string key);

        void Clear();
    }
}
