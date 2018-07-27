namespace MyHandmadeWebServer
{
    using ByTheCake;

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

            var application = new ByTheCakeApplication();
            application.InitializeDatabase();
            application.Configure(appRouteConfig);

            this.webServer = new WebServer(8230, appRouteConfig);
            this.webServer.Run();
        }
    }
}
