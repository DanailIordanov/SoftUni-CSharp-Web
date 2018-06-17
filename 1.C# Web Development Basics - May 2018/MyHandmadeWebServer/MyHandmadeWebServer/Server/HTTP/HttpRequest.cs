namespace MyHandmadeWebServer.Server.Http
{
    using MyHandmadeWebServer.Server.Enums;
    using MyHandmadeWebServer.Server.Exceptions;
    using MyHandmadeWebServer.Server.Http.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.Headers = new HttpHeaderCollection();
            this.UrlParameters = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IDictionary<string, string> FormData { get; private set; }
        
        public void AddUrlParameter(string key, string value)
        {
            this.UrlParameters.Add(key, value);
        }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = requestLines[0].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("Invalid request line");
            }

            this.RequestMethod = this.ParseRequestMethod(requestLine[0]);
            this.Url = requestLine[1];
            this.Path = this.Url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.POST)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this.FormData);
            }
        }

        private HttpRequestMethod ParseRequestMethod(string requestMethod)
        {
            try
            {
                Enum.TryParse(requestMethod, true, out HttpRequestMethod method);
                return method;
            }
            catch (ArgumentException)
            {
                throw new BadRequestException("Invalid request method");
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            var endIndex = Array.IndexOf(requestLines, string.Empty);
            for (int i = 0; i < endIndex; i++)
            {
                var headerArgs = requestLines[i].Split(": ", StringSplitOptions.None);

                var header = new HttpHeader(headerArgs[0], headerArgs[1]);
                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                throw new BadRequestException("Missing a host header");
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var query = this.Url.Split("?")[1];
            ParseQuery(query, this.QueryParameters);
        }

        private void ParseQuery(string query, IDictionary<string, string> dictionary)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var queryPairs = query.Split("&");

            foreach (var pair in queryPairs)
            {
                var pairArgs = pair.Split("=");
                if (pairArgs.Length != 2)
                {
                    continue;
                }

                var key = pairArgs[0];
                var value = pairArgs[1];

                dictionary.Add(WebUtility.UrlDecode(key), WebUtility.UrlDecode(value));
            }
        }
    }
}
