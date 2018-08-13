namespace Mvc.Framework
{
    using WebServer;

    using System;
    using System.Reflection;

    public static class MvcEngine
    {
        public static void Run(WebServer server)
        {
            RegisterAssemblyName();
            RegisterControllersData();
            RegisterViewsData();
            RegisterModelsData();

            try
            {
                server.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void RegisterAssemblyName()
        {
            MvcContext.Get.AssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        }

        private static void RegisterControllersData()
        {
            MvcContext.Get.ControllersFolder = "Controllers";
            MvcContext.Get.ControllersSuffix = "Controller";
        }

        private static void RegisterViewsData()
        {
            MvcContext.Get.ViewsFolder = "Views";
        }

        private static void RegisterModelsData()
        {
            MvcContext.Get.ModelsFolder = "Models";
        }
    }
}
