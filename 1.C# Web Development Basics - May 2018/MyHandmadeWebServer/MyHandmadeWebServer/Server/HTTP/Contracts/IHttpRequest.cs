namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using Enums;

    using System.Collections.Generic;

    public interface IHttpRequest
    {
        HttpRequestMethod Method { get; }
                
        string Url { get; }

        IDictionary<string, string> UrlParameters { get; }

        string Path { get; }

        IDictionary<string, string> QueryParameters { get; }

        IHttpHeaderCollection Headers { get; }

        IDictionary<string, string> FormData { get; }

        void AddUrlParameter(string key, string value);
    }
}