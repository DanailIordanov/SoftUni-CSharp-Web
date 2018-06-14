namespace MyHandmadeWebServer.Server.HTTP.Contracts
{
    using MyHandmadeWebServer.Server.Enums;

    using System.Collections.Generic;

    public interface IHttpRequest
    {
        IDictionary<string, string> FormData { get; }

        IHttpHeaderCollection Headers { get; }

        string Path { get; }

        IDictionary<string, string> QueryParameters { get; }

        HttpRequestMethod RequestMethod { get; }

        string Url { get; }

        IDictionary<string, string> UrlParameters { get; }

        void AddUrlParameter(string key, string value);
    }
}
