namespace MyHandmadeWebServer.Server.Routing
{
    using Contracts;
    using Enums;
    using Handlers.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>>();

            foreach (var requestMethod in Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>())
            {
                this.routes.Add(requestMethod, new Dictionary<string, IRequestHandler>());
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes => this.routes;

        public void AddRoute(string route, IRequestHandler requestHandler)
        {
            if (requestHandler.GetType().ToString().ToLower().Contains("get"))
            {
                this.routes[HttpRequestMethod.Get].Add(route, requestHandler);
            }
            else if (requestHandler.GetType().ToString().ToLower().Contains("post"))
            {
                this.routes[HttpRequestMethod.Post].Add(route, requestHandler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler.");
            }
        }
    }
}