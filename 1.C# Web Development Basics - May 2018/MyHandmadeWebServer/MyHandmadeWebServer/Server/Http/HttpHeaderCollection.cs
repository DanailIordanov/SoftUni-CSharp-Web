namespace MyHandmadeWebServer.Server.Http
{
    using Common;
    using Contracts;

    using System;
    using System.Collections.Generic;
    using System.Text;

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
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            return this.headers.ContainsKey(key);
        }

        public HttpHeader Get(string key)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key '{key}' is not present in the headers collection.");
            }

            return this.headers[key];
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var header in this.headers)
            {
                result.AppendLine(header.Value.ToString());
            }

            return result.ToString();
        }
    }
}