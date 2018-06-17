namespace MyHandmadeWebServer.Server.Routing.Contracts
{
    using MyHandmadeWebServer.Server.Handlers.Contracts;

    using System.Collections.Generic;

    public interface IRoutingContext
    {
        IEnumerable<string> Parameters { get; }

        IRequestHandler RequestHandler { get; }
    }
}
