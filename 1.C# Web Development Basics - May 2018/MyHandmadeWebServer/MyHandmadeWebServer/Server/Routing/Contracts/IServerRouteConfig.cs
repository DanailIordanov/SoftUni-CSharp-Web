namespace MyHandmadeWebServer.Server.Routing.Contracts
{
    using Enums;

    using System.Collections.Generic;

    public interface IServerRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes { get; }
    }
}