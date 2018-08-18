namespace WebServer.Http.Contracts
{
    public interface IHttpSession
    {
        string Id { get; }

        void Add(string key, object value);

        bool ContainsKey(string key);

        void Clear();

        object Get(string key);

        T Get<T>(string key);
    }
}
