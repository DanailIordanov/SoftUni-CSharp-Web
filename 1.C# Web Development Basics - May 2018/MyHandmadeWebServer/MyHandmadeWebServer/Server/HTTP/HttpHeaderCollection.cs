namespace MyHandmadeWebServer.Server.HTTP
{
    using MyHandmadeWebServer.Server.HTTP.Contracts;

    using System.Collections.Generic;
    using System.Linq;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            this.headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
            if (this.headers.ContainsKey(key))
            {
                return true;
            }

            return false;
        }

        public HttpHeader GetHeader(string key)
        {
            return this.headers.First(h => h.Key == key).Value;
        }

        public override string ToString()
        {
            return string.Join("\n", this.headers);
        }
    }
}
