﻿namespace MyHandmadeWebServer.Server.Http.Contracts
{
    using Enums;

    using System.Collections.Generic;

    public interface IHttpRequest
    {
        HttpRequestMethod Method { get; }
                
        string Url { get; }

        IDictionary<string, string> UrlParameters { get; }

        string Path { get; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        IDictionary<string, string> FormData { get; }

        IHttpSession Session { get; set; }

        void AddUrlParameter(string key, string value);
    }
}