namespace MyHandmadeWebServer.Server.Http
{
    using Common;
    using Contracts;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, ICollection<HttpHeader>> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, ICollection<HttpHeader>>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            var headerKey = header.Key;

            if (!this.headers.ContainsKey(headerKey))
            {
                this.headers[headerKey] = new List<HttpHeader>();
            }

            this.headers[headerKey].Add(header);
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.headers.ContainsKey(key);
        }

        public ICollection<HttpHeader> Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key '{key}' is not present in the headers collection.");
            }

            return this.headers[key];
        }

        public IEnumerator<ICollection<HttpHeader>> GetEnumerator() => this.headers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.headers.Values.GetEnumerator();

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var header in this.headers)
            {
                foreach (var headerValue in header.Value)
                {
                    result.AppendLine(headerValue.ToString());
                }
            }

            return result.ToString();
        }
    }
}