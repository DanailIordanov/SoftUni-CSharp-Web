namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpCookieCollection : IEnumerable<HttpCookie>
    {
        void Add(HttpCookie cookie);

        bool ContainsKey(string key);

        HttpCookie Get(string key);
    }
}