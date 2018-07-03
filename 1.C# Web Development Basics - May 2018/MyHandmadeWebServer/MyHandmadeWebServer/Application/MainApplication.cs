namespace MyHandmadeWebServer.Application
{
    using Application.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get("/", request => new HomeController().Index());
            appRouteConfig.Get("/register", request => new UserController().RegisterGet());
            appRouteConfig.Post("/register", request => new UserController().RegisterPost(request.FormData["name"]));
            appRouteConfig.Get("/user/{(?<name>\\w+)}", request => new UserController().Details(request.UrlParameters["name"]));
        }
    }
}
