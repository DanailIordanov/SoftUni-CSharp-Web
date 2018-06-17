namespace MyHandmadeWebServer.Server.Routing
{
    using MyHandmadeWebServer.Server.Handlers.Contracts;
    using MyHandmadeWebServer.Server.Routing.Contracts;

    using System.Collections.Generic;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(IRequestHandler handler, IEnumerable<string> parameters)
        {
            this.RequestHandler = handler;
            this.Parameters = parameters;
        }

        public IRequestHandler RequestHandler { get; private set; }

        public IEnumerable<string> Parameters { get; private set; }
    }
}
