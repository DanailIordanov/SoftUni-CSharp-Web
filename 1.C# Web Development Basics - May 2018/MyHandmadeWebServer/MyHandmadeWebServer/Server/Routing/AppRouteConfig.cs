namespace MyHandmadeWebServer.Server.Routing
{
    using Contracts;
    using Enums;
    using Handlers;
    using Handlers.Contracts;
    using Http.Contracts;
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
                this.routes[requestMethod] = new Dictionary<string, IRequestHandler>();
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRequestHandler>> Routes => this.routes;

        public void Get(string route, Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            this.AddRoute(route, HttpRequestMethod.Get, new RequestHandler(handlingFunc));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handlingFunc)
        {
            this.AddRoute(route, HttpRequestMethod.Post, new RequestHandler(handlingFunc));
        }

        private void AddRoute(string route, HttpRequestMethod requestMethod, IRequestHandler requestHandler)
        {
            this.routes[requestMethod].Add(route, requestHandler);
        }
    }
}