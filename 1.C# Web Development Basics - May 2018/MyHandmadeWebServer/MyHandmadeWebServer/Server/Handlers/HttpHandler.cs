namespace MyHandmadeWebServer.Server.Handlers
{
    using MyHandmadeWebServer.Server.Handlers.Contracts;
    using MyHandmadeWebServer.Server.Http.Contracts;
    using MyHandmadeWebServer.Server.Routing.Contracts;

    using System.Text.RegularExpressions;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            var requestMethod = context.Request.RequestMethod;
            var requestPath = context.Request.Path;

            foreach (var registeredRoute in this.serverRouteConfig.Routes[requestMethod])
            {
                var routePattern = registeredRoute.Key;
                var routingContext = registeredRoute.Value;

                var regex = new Regex(routePattern);
                var match = regex.Match(requestPath);

                if (!match.Success)
                {
                    continue;
                }

                var parameters = routingContext.Parameters;

                foreach (var parameter in parameters)
                {
                    var parameterValue = match.Groups[parameter].Value;
                    context.Request.AddUrlParameter(parameter, parameterValue);
                }

                return routingContext.RequestHandler.Handle(context);
            }
            //TODO
            return null;
        }
    }
}
