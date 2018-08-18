namespace Mvc.Application
{
    using Mvc.Framework;
    using Mvc.Framework.Routers;
    using WebServer;

    public class Launcher
    {
        public static void Main()
        {
            var server = new WebServer(9230, new ControllerRouter(), new ResourceRouter());
            MvcEngine.Run(server);
        }
    }
}
