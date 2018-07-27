namespace MyHandmadeWebServer.Server.Handlers
{
    using Common;
    using Contracts;
    using Http;
    using Http.Contracts;
    using Http.Response;
    using Routing.Contracts;

    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            try
            {
                var anonymousPaths = new[] { "/login", "/register" };

                if (!anonymousPaths.Contains(context.Request.Path)
                    && (context.Request.Session == null || !context.Request.Session.ContainsKey(SessionStore.CurrentUserKey)))
                {
                    return new RedirectResponse(anonymousPaths.First());
                }

                var requestMethod = context.Request.Method;
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
            }
            catch (Exception ex)
            {
                return new InternalServerErrorResponse(ex);
            }
            
            return new NotFoundResponse();
        }
    }
}