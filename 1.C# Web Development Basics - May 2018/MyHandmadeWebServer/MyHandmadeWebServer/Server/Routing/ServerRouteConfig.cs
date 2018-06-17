namespace MyHandmadeWebServer.Server.Routing
{
    using MyHandmadeWebServer.Server.Enums;
    using MyHandmadeWebServer.Server.Routing.Contracts;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            foreach (var requestMethod in Enum.GetValues(typeof(HttpRequestMethod)).Cast<HttpRequestMethod>())
            {
                this.routes[requestMethod] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;

        private void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var requestMethodWithRoutes in appRouteConfig.Routes)
            {
                var requestMethod = requestMethodWithRoutes.Key;
                var routesWithHandlers = requestMethodWithRoutes.Value;

                foreach (var routeWithHandler in routesWithHandlers)
                {
                    var parameters = new List<string>();

                    var route = routeWithHandler.Key;
                    var requestHandler = routeWithHandler.Value;

                    var parsedRouteRegex = this.ParseRoute(route, parameters);

                    var routingContext = new RoutingContext(requestHandler, parameters);

                    this.Routes[requestMethod].Add(parsedRouteRegex, routingContext);
                    
                }
            }
        }

        private string ParseRoute(string route, List<string> parameters)
        {
            if (route == "/")
            {
                return "^/$";
            }

            var parsedRegex = new StringBuilder();
            parsedRegex.Append("^/");

            var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            this.ParseTokens(tokens, parameters, parsedRegex);

            return parsedRegex.ToString();
        }

        private void ParseTokens(string[] tokens, List<string> parameters, StringBuilder parsedRegex)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                var end = i == tokens.Length - 1 ? "$" : "/";
                var currentToken = tokens[i];

                if (!currentToken.StartsWith("{") && !currentToken.EndsWith("}"))
                {
                    parsedRegex.Append($"{currentToken}{end}");
                    continue;
                }

                var regex = new Regex(@"<\w+>");
                var match = regex.Match(currentToken);

                if (!match.Success)
                {
                    continue;
                }

                var parameter = match.Value.Substring(1, match.Length - 2);
                parameters.Add(parameter);

                parsedRegex.Append($"{currentToken.Substring(1, currentToken.Length - 2)}{end}");
            }
        }
    }
}
