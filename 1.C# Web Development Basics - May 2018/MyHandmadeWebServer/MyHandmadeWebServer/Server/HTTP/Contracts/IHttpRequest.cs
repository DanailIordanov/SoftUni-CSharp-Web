namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using MyHandmadeWebServer.Server.Enums;

    using System.Collections.Generic;

    public interface IHttpRequest
    {
        HttpRequestMethod RequestMethod { get; }
                
        string Url { get; }

        IDictionary<string, string> UrlParameters { get; }

        string Path { get; }

        IDictionary<string, string> QueryParameters { get; }

        IHttpHeaderCollection Headers { get; }

        IDictionary<string, string> FormData { get; }

        void AddUrlParameter(string key, string value);
    }
}
