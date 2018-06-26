namespace MyHandmadeWebServer.Server.Routing.Contracts
{
    using Enums;
    using Handlers.Contracts;

    using System.Collections.Generic;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes { get; }

        void AddRoute(string route, IRequestHandler requestHandler);
    }
}