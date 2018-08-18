namespace WebServer.Http
{
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        private readonly string requestText;

        public HttpRequest(string requestText)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestText, nameof(requestText));

            this.requestText = requestText;

            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.UrlParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();

            this.ParseRequest(requestText);
        }

        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public string Path { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public IHttpSession Session { get; set; }

        public IDictionary<string, string> FormData { get; private set; }

        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (!requestLines.Any())
            {
                throw new BadRequestException("Invalid request.");
            }

            var requestLine = requestLines.First().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line.");
            }

            this.Method = this.ParseRequestMethod(requestLine[0]);
            this.Url = requestLine[1];
            this.Path = this.ParsePath(this.Url);

            this.ParseHeaders(requestLines);
            this.ParseCookies();
            this.ParseParameters();

            if (this.Method == HttpRequestMethod.Post)
            {
                this.ParseQuery(requestLines.Last(), this.FormData);
            }

            this.SetSession();
        }

        private HttpRequestMethod ParseRequestMethod(string requestMethod)
        {
            try
            {
                return Enum.Parse<HttpRequestMethod>(requestMethod, true);
            }
            catch (Exception)
            {
                throw new BadRequestException("Invalid request method.");
            }
        }

        private string ParsePath(string url)
        {
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestLines)
        {
            var endIndex = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < endIndex; i++)
            {
                var headerArgs = requestLines[i].Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerArgs.Length != 2)
                {
                    throw new BadRequestException($"The request contains an invalid header.");
                }

                var header = new HttpHeader(headerArgs[0], headerArgs[1].Trim());
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey(HttpHeader.Host))
            {
                throw new BadRequestException("Missing a host header.");
            }
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.Headers.Get(HttpHeader.Cookie);

                foreach (var cookie in allCookies)
                {
                    var cookieParts = cookie.Value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (!cookieParts.Any() || !cookieParts.Any(c => c.Contains("=")))
                    {
                        continue;
                    }

                    foreach (var cookiePart in cookieParts)
                    {
                        var cookieKeyValuePair = cookiePart.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValuePair.Length == 2)
                        {
                            var key = cookieKeyValuePair[0].Trim();
                            var value = cookieKeyValuePair[1].Trim();

                            this.Cookies.Add(new HttpCookie(key, value, false));
                        }
                    }
                }
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)[1];
            ParseQuery(query, this.UrlParameters);
        }

        private void ParseQuery(string query, IDictionary<string, string> dictionary)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var queryPairs = query.Split(new[] { '&' });

            foreach (var pair in queryPairs)
            {
                var pairArgs = pair.Split(new[] { '=' });
                if (pairArgs.Length != 2)
                {
                    continue;
                }

                var key = WebUtility.UrlDecode(pairArgs[0]);
                var value = WebUtility.UrlDecode(pairArgs[1]);

                dictionary.Add(key, value);
            }
        }

        private void SetSession()
        {
            if (this.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var cookie = this.Cookies.Get(SessionStore.SessionCookieKey);
                var sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }

        public override string ToString() => this.requestText;
    }
}
