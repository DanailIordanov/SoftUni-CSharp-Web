namespace MyHandmadeWebServer.Server.Http.Contracts
{
    public interface IHttpSession
    {
        string Id { get; }

        bool ContainsKey(string key);

        object Get(string key);

        T Get<T>(string key);

        void Add(string key, object value);

        void Clear();
    }
}
