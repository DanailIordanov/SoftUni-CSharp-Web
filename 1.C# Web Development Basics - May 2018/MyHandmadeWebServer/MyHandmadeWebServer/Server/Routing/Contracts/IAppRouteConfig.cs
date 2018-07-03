namespace MyHandmadeWebServer.Server.Routing.Contracts
{
    using Enums;
    using Handlers.Contracts;
    using Http.Contracts;

    using System;
    using System.Collections.Generic;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes { get; }

        void Get(string route, Func<IHttpRequest, IHttpResponse> handlingFunc);

        void Post(string route, Func<IHttpRequest, IHttpResponse> handlingFunc);
    }
}