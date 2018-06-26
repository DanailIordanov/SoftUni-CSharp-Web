namespace MyHandmadeWebServer.Server.Routing
{
    using Common;
    using Contracts;
    using Handlers.Contracts;

    using System.Collections.Generic;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(IRequestHandler handler, IEnumerable<string> parameters)
        {
            CoreValidator.ThrowIfNull(handler, nameof(handler));
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));

            this.RequestHandler = handler;
            this.Parameters = parameters;
        }

        public IRequestHandler RequestHandler { get; private set; }

        public IEnumerable<string> Parameters { get; private set; }
    }
}