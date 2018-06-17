namespace MyHandmadeWebServer
{
    using MyHandmadeWebServer.Server;
    using MyHandmadeWebServer.Server.Contracts;
    using MyHandmadeWebServer.Server.Routing;

    using System;

    public class Launcher : IRunnable
    {
        private WebServer webServer;

        public static void Main(string[] args)
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var routeConfig = new AppRouteConfig();
            this.webServer = new WebServer(8230, routeConfig);
            this.webServer.Run();
        }
    }
}
