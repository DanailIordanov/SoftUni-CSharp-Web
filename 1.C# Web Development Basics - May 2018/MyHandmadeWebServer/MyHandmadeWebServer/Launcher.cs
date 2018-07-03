namespace MyHandmadeWebServer
{
    using Application;
    using Server;
    using Server.Contracts;
    using Server.Routing;

    public class Launcher : IRunnable
    {
        private WebServer webServer;

        public static void Main(string[] args)
        {
            new Launcher().Run();
        }

        public void Run()
        {
            var appRouteConfig = new AppRouteConfig();
            var mainApplication = new MainApplication();
            mainApplication.Configure(appRouteConfig);

            this.webServer = new WebServer(8230, appRouteConfig);
            this.webServer.Run();
        }
    }
}
